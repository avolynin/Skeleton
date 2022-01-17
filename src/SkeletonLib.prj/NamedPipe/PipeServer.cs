using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Mallenom.SkeletonLib.NamedPipe
{
	// Delegate for passing received message back to caller
	public delegate void DelegateMessage(string Reply);

	public class PipeServer
	{
		private NamedPipeServerStream _pipeServer;
		private bool _pipeIsOpen = true;

		public event DelegateMessage PipeMessage;
		public string Name { get; set; }
		public Queue<Task> QueueTasks;

		public PipeServer(string name)
		{
			Name = name;
			QueueTasks = new Queue<Task>();
			_pipeServer = new NamedPipeServerStream(Name,
				   PipeDirection.InOut, -1, PipeTransmissionMode.Byte);
		}

		public void SendAsync(byte[] bytes)
		{
			var task = new Task(() => { Send(bytes); });
			QueueTasks.Enqueue(task);

			if(_pipeIsOpen)
			{
				QueueTasks.Dequeue().Start();
				_pipeIsOpen = false;
			}
		}

		public void ListenAsync()
		{
			var task = new Task(() => { Listen(); });
			QueueTasks.Enqueue(task);

			if(_pipeIsOpen)
			{
				QueueTasks.Dequeue().Start();
				_pipeIsOpen = false;
			}
		}

		public void Send(byte[] bytes)
		{
			try
			{
				Console.WriteLine("Send");

				_pipeServer.WaitForConnection();
				_pipeServer.BeginWrite(bytes, 0, bytes.Length,
					new AsyncCallback(WriteCallback), _pipeServer);
			}
			catch(Exception)
			{
				throw;
			}
		}

		private void WriteCallback(IAsyncResult iar)
		{
			try
			{
				// Получение записывающего канала
				var pipeStream = (NamedPipeServerStream)iar.AsyncState;

				// Конец записи
				pipeStream.EndWrite(iar);
				pipeStream.Flush();

				if(QueueTasks.TryDequeue(out var task)) task.Start();
				else _pipeIsOpen = true;
			}
			catch(Exception)
			{
				throw;
			}
		}

		public void Listen()
		{
			try
			{
				// Ожидание соединения
				_pipeServer.BeginWaitForConnection
				(new AsyncCallback(WaitForConnectionCallBack), _pipeServer);
			}
			catch(Exception)
			{
				throw;
			}
		}

		private void WaitForConnectionCallBack(IAsyncResult iar)
		{
			try
			{
				// Получение канала
				NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState;
				// End waiting for the connection
				//pipeServer.EndWaitForConnection(iar);

				byte[] buffer = new byte[255];

				// Read the incoming message
				pipeServer.Read(buffer, 0, 255);

				// Convert byte buffer to string
				string stringData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

				// Pass message back to calling form
				PipeMessage.Invoke(stringData);

				if(QueueTasks.TryDequeue(out var task)) task.Start();
				else _pipeIsOpen = true;
			}
			catch
			{
				return;
			}
		}

		public void Kill()
		{
			// Kill original sever
			_pipeServer.Close();
			_pipeServer.Dispose();
		}
	}
}

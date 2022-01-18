using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Mallenom.SkeletonLib.NamedPipe
{
	/// <summary>Именованный канал для синхронной записи и чтения по каналу.</summary>
	public class PipeServer : IDisposable
	{
		#region Fields

		/// <summary>Экземпляр именованного канала-сервера.</summary>
		private NamedPipeServerStream _pipeServer;

		/// <summary>Имя канала.</summary>
		public string Name { get; set; }

		#endregion

		/// <summary>Создает именованный канал с указанным именем.</summary>
		/// <param name="name">Имя канала.</param>
		public PipeServer(string name)
		{
			Name = name;
			_pipeServer = new NamedPipeServerStream(Name,
				PipeDirection.InOut, -1, PipeTransmissionMode.Byte);
		}

		/// <summary>Отправляет массив байтов по каналу клиентам.</summary>
		/// <param name="bytes">Отправляемый массив байтов.</param>
		public void Send(byte[] bytes)
		{
			try
			{
				_pipeServer.WaitForConnection();
				_pipeServer.BeginWrite(bytes, 0, bytes.Length,
					new AsyncCallback(WriteCallback), null);
			}
			catch(Exception)
			{
				throw;
			}
		}

		/// <summary>Прослушивает сообщения клиентов.</summary>
		/// <returns>Массив байтов полученный от клиента.</returns>
		public byte[] Listen()
		{
			// Обертка задачи для получения результата обратного вызова(callback)
			var taskCompletionSource = new TaskCompletionSource<byte[]>();

			try
			{
				// Ожидание соединения
				_pipeServer.BeginWaitForConnection(asyncResult =>
				{
					taskCompletionSource.SetResult(WaitForConnectionCallBack(asyncResult));
				}, null);
			}
			catch(Exception)
			{
				throw;
			}

			return taskCompletionSource.Task.Result;
		}

		/// <summary>Вызывается после окончания записи.</summary>
		/// <param name="iar">Представляет состояние асинхронной операции.</param>
		private void WriteCallback(IAsyncResult iar)
		{
			try
			{
				// Конец записи
				_pipeServer.EndWrite(iar);
				_pipeServer.Flush();
			}
			catch(Exception)
			{
				throw;
			}
		}

		/// <summary>Вызывается после подключения клиента.</summary>
		/// <param name="iar">Представляет состояние асинхронной операции.</param>
		/// <returns>Массив прочитанных байтов.</returns>
		private byte[] WaitForConnectionCallBack(IAsyncResult iar)
		{
			try
			{
				byte[] buffer = new byte[100000];
				// Чтение входящего сообщения в буффер
				_pipeServer.Read(buffer, 0, 100000);

				return buffer;
			}
			catch
			{
				return null;
			}
		}

		#region [IDisplosable pattern]

		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if(!disposedValue)
			{
				if(disposing)
				{
					
				}
				_pipeServer.Close();
				_pipeServer = null;

				disposedValue = true;
			}
		}

		~PipeServer()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}

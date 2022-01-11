import time
import sys
import win32pipe, win32file, pywintypes

print("pipe client")
quit = False

win32pipe.CreateNamedPipe

while not quit:
    try:
        handle = win32file.CreateFile(
            r'\\.\pipe\testpipe',
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0,
            None,
            win32file.OPEN_EXISTING,
            0,
            None
        )
        
        res = win32pipe.SetNamedPipeHandleState(handle, win32pipe.PIPE_READMODE_BYTE, None, None)
        print(res)
        if res == 0:
            print(f"SetNamedPipeHandleState return code: {res}")
        while True:
            resp = win32file.ReadFile(handle, 64*1024)
            print(f"message: {resp}")
    except pywintypes.error as e:
        if e.args[0] == 2:
            print("no pipe, trying again in a sec")
            time.sleep(1)
        elif e.args[0] == 109:
            print("broken pipe")
            quit = True
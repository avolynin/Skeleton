import time
import win32pipe, win32file, pywintypes
import io
from PIL import Image

print("pipe client")
quit = False

def byteToImage(bytesImg):
    imageStream = io.BytesIO(bytesImg)
    imageFileJPG = Image.open(imageStream).convert("RGB")
    imageFileJPG.show();

    imageStream.close()

while not quit:
    try:
        handle = win32file.CreateFile(
            r'\\.\pipe\testpipe',
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0, None,
            win32file.OPEN_EXISTING, 0, None)
        res = win32pipe.SetNamedPipeHandleState(handle, win32pipe.PIPE_READMODE_BYTE, None, None)

        if res == 0:
            print(f"SetNamedPipeHandleState return code: {res}")
        rc, data = win32file.ReadFile(handle, 64*1024)
        print(f"message: {data}")
        byteToImage(data)
    except pywintypes.error as e:
        if e.args[0] == 2:
            print("no pipe, trying again in a sec")
            time.sleep(1)
        elif e.args[0] == 109:
            print("broken pipe")
            quit = True
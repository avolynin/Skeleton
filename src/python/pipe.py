import time
import win32pipe, win32file, pywintypes
import io
import ctypes
from PIL import Image

quit = False
namePipe = r'testpipe'
test = 0

def initPipe():
    handle = win32file.CreateFile(r'\\.\pipe\testpipe',
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0, None, win32file.OPEN_EXISTING, 0, None)

    res = win32pipe.SetNamedPipeHandleState(handle, 
        win32pipe.PIPE_TYPE_BYTE, None, None)
    
    if res == 0:
            ctypes.windll.user32.MessageBoxW(0, u"Error", u"SetNamedPipeHandleState return code: "+ str(res), 0)
    
    return handle

def byteToImage(bytesImg):
    imageStream = io.BytesIO(bytesImg)
    imageFileJPG = Image.open(imageStream).convert("RGB")
    imageStream.close()

    return imageFileJPG;

while not quit:
    try:
        handle = initPipe()
        
        rc, data = win32file.ReadFile(handle, 64*1024)
        testImg = byteToImage(data)
        
        testImg = testImg.convert('L')
        buf = io.BytesIO()
        testImg.save(buf, format='JPEG')
        
        win32file.WriteFile(handle, buf.getvalue())
        
        quit = True
    except pywintypes.error as e:
        if e.args[0] == 2:
            # Канал не сущесвует
            time.sleep(1)
        elif e.args[0] == 109:
            quit = True
            ctypes.windll.user32.MessageBoxW(0, u"Error", u"Broken pipe.", 0)
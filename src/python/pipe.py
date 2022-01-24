import time
import win32pipe, win32file, pywintypes
import ctypes

class ClientPipe():
    def __init__(self, name):
        self.name = name
        self.__handle = win32file.CreateFile(name,
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0, None, win32file.OPEN_EXISTING, 0, None)

        self.__res = win32pipe.SetNamedPipeHandleState(self.__handle, 
            win32pipe.PIPE_TYPE_BYTE, None, None)
        
        if self.__res == 0:
            ctypes.windll.user32.MessageBoxW(0, u"Error", u"SetNamedPipeHandleState return code: "+ str(self.__res), 0)
        
    def send(self, byteArr):
        quit = False
        while not quit:
            try:
                win32file.WriteFile(self.__handle, byteArr)
                quit = True
            except pywintypes.error as e:
                if e.args[0] == 2:
                    # Канал не сущесвует
                    time.sleep(1)
                elif e.args[0] == 109:
                    quit = True
                    ctypes.windll.user32.MessageBoxW(0, u"Error", u"Broken pipe.", 0) 
        
    def listen(self, buffSize):
        quit = False
        while not quit:
            try:
                rc, data = win32file.ReadFile(self.__handle, buffSize)
                quit = True
            except pywintypes.error as e:
                if e.args[0] == 2:
                    # Канал не сущесвует
                    time.sleep(1)
                elif e.args[0] == 109:
                    quit = True
                    ctypes.windll.user32.MessageBoxW(0, u"Error", u"Broken pipe.", 0) 
        
        return data
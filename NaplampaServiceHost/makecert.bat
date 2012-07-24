echo FONTOS: read access-t kell adni a "NETWORK SERVICE"-nek, az adott kulcsra. A kulcsok Vistan a "c:\Users\All Users\Microsoft\Crypto\RSA\MachineKeys\" konyvtarban vannak. A megfelelo fajl megtalalasahoz letezik egy FindPrivateKey.exe valahol, de ezt nem tudtam hasznalni.


"c:\Program Files\Microsoft SDKs\Windows\v6.0\Bin\x64\makecert.exe" -sr LocalMachine -ss My -a sha1 -n "CN=magmalight.com" -sky exchange MagmaLight.cer -pe

rem "c:\Program Files\Microsoft SDKs\Windows\v6.0\Bin\x64\pvk2pfx.exe" -pvk MagmaLight.pvk -spc MagmaLight.cer -pfx MagmaLight.pfx 


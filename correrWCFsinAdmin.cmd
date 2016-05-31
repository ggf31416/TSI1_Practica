REM Correr este script como admin
REM sddl=D:(A;;GX;;;S-1-1-0) supuestamente significa todos los usuarios
SET USER=sddl=D:(A;;GX;;;S-1-1-0)
netsh http delete urlacl url=http://+:8835/tsi/
netsh http delete urlacl url=http://+:8836/tsi/
netsh http delete urlacl url=http://+:8837/tsi/
netsh http delete urlacl url=http://+:8838/tsi/
netsh http delete urlacl url=http://+:8839/tsi/

netsh http add urlacl url=http://+:8835/tsi/ %USER%
netsh http add urlacl url=http://+:8836/tsi/ %USER%
netsh http add urlacl url=http://+:8837/tsi/ %USER%
netsh http add urlacl url=http://+:8838/tsi/ %USER%
netsh http add urlacl url=http://+:8839/tsi/ %USER%
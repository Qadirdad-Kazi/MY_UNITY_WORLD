@echo off
echo Extracting Jeep and Lada...
powershell -NoProfile -Command "Expand-Archive -Force 'D:\Unity\MyFantasyWorld\Assets\Game_Models\jeep-gladiator\source\FINAL_MODEL.zip' 'D:\Unity\MyFantasyWorld\Assets\Game_Models\jeep-gladiator\source\extracted'"
powershell -NoProfile -Command "Expand-Archive -Force 'D:\Unity\MyFantasyWorld\Assets\Game_Models\lada-2107\source\2107.zip' 'D:\Unity\MyFantasyWorld\Assets\Game_Models\lada-2107\source\extracted'"

echo Deleting Downloads starter pack (~5.6 GB)...
rd /s /q "%USERPROFILE%\Downloads\MyFantasyWorld_StarterPack"
del /f /q "%USERPROFILE%\Downloads\SECTION_BY_SECTION_BUILD_BIBLE.pdf" 2>nul
del /f /q "%USERPROFILE%\Downloads\FROM_SCRATCH_WORLD_BUILDING_GUIDE.pdf" 2>nul
del /f /q "%USERPROFILE%\Downloads\DELETE_OLD_My_World_Game.bat" 2>nul
del /f /q "%USERPROFILE%\Downloads\WORLD_BUILDING_GUIDE.pdf" 2>nul

echo Done.
echo Jeep/Lada extracted under Game_Models ...\source\extracted
echo Downloads starter pack removed.
pause

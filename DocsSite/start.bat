@echo off
cd /d "%~dp0"
echo Installing dependencies (first run only)...
python -m pip install -r requirements.txt -q
echo.
echo Starting docs site (reachable on this PC and LAN)...
echo This PC:  http://127.0.0.1:5050
echo Other laptop: use the LAN link printed below
echo Press Ctrl+C to stop.
echo.
python app.py
pause

# MMOnkey

### Configure server:
- Open: D:\Projects\ThesisMMO\deploy\MMOServer\bin\MMOServer.dll.config
- Evaluation Mode and Entity Scenario parameters are interesting

#### Evaluation Mode
- Server starts with entities not moving
- Once in the game world press 'p' to start the evaluation.
- Evaluation takes 10s. Don't move your character during that time.
- The evaluation logs will be placed in ThesisMMO\deploy\UnityClient
- If the entity is equipped with Distracting Shot, it will be fired after 5s during evaluation to change frequency table
- Server should be restarted before performing a new evaluation.

#### Entity Scenario
- Choose between 0 or 1
- 0: Exploration Scenario
- 1: Crowded Scenario

#### Starting server
1.	Start server in ThesisMMO\deploy\bin_Win64\PhotonControl.exe
2.	Go to the system tray and open photon control, and click on MMOServer -> Start as Application

### Client
1. Start the client in ThesisMMO\deploy\UnityClient\UClient.exe
2. Click connect. (No address needed if server is running on local machine)
3. Create your character and enter the game world.

#### Tutorial
- W-A-S-D to move character
- Rightclick to Autoattack
- Press 1-2-3 to use a skill
- Press 'u' to display frequency plane (game lags when frequency table is redrawn)
- Press 'i' and 'o' to display game statistics (message counter only gives plausible values if entities are moving)
- Press 'p' to start evaluation if in evaluation mode.

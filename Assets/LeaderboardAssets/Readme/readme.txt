Test Global Leaderboard SDK manual (Ver. 0.0.2)  - Latest release date Feb 19, 2021       

1. Subject
   Publishing the user's game score on the leaderboard which is connected to the test server for test purposes.


2. Test Global Leaderboard URL 
   http://score.iircade.com/ranking/ranking_test.php


3. Protocol
   Http post 


4. Data structure (the variable must be lowercase characters.)

   no.  variable    type          comment
   1    request     string        'save'   - uploads game score to server
                                  'load'   - downloads score list from server
                                  'delete' - delete game score list at server
   2    game        string[8]     iiRcade provides game id (if no id, make any id in your project - test server only)
   3    user        string[32]    user name
   4    lives       int[2]        number of lives (e.g. 3 or 5) if necessary, use it for other purposes
   5    difficulty  int[11]       game difficulty. if necessary, use it for other purposes 
   6    stage       int[11]       user's last stage. if necessary, use it for other purposes
   7    score       int[11]	  user's score
   8    mac         string[10]    game console MAC address. (see sample codes) 

   - Game id can be obtained from iiRcade. If no id, make any id in your project - test server only.
   - The server gets user country information by user's public IP, iiRcade server gets user IP automatically. 
   - If you need more fields (e.g. coins, health, etc), request them to iiRcade.


5. MAC address
   - Device Console MAC address is 12 characters, but only the 3rd to 10 characters use here. 


6. Upload Score
   The score will be uploaded to the server when the game is over.
   - The same scores by the same user name will be saved on the server as one record. 
   - Save, load, delete use POST method.

   request = 'save' 
   game = game_id		// if no id, make any id in your project - test server only
   user = 'testman'
   score = 12345
   lives = 3 
   mac = game.macAddr

- 'difficulty', 'stage' are optional fields, so if necessary, use it for other purposes or ignore them.
- Server ignores field order, but field names must be lowercase characters.


7. Download Score
   Download score needs 'request' and 'game' field.

   request = 'load' 
   game = game_id		// if no id, make any id in your project - test server only


8. Downloaded score format
   Downloaded score is a single string and it is divided by '\n' and '|'. The maximum list is 30. 

   '\n' : divide user
   '|'  : divide field

     date|country|user name|lives|score|difficulty|stage\ndate|country|user name|lives|score|difficulty|stage\n¡¦
     e.g.) Nov.07.2020|USA|userMicle|3|23456|0|0|\nOct.22.2020|KOR|park|5|12345|0\0\n¡¦

- if not used 'difficulty', 'stage' fields, always returns value 0.


9. Delete test data at server
   Delete records needs 'request', 'game' and 'user' field, user field is optional. 

   request = 'delete' 
   game = game_id		// delete all records

   request = 'delete' 
   game = 'SAMPLE'		
   user = 'testman'		// delete all records of 'testman'

----------------------------------------------------------------------------------

This demo package shows how to use a Virtual keyboard and Global Leaderboard.

In Unity, Unreal, Game Maker Studio, use keyboard and PS4 game controller.
In Unity, Unreal, Game Maker Studio, if use mouse button both, set Input Manager Submit section like this

  [Submit]
     Positive Button : joystick button 0  <- calls iiRcade game console [A] button(or PS4 [Square] button) submit event
     Alt Positive Button : mouse 0        <- calls left mouse button click event 

  [Warning] These buttons use carefully.
  - Input Manager 'return' calls the iiRcade game console [Start] button event. 
  - Input Manager 'space'  calls the iiRcade game console [X] button event. 
  - Input Manager 'shift'  calls the iiRcade game console [Y] button event. 

Virtual keyboard keys use submit events, so remove(or replace) the 'return' keyword from all submit sections in the Input Manager.
If not removed, both the iiRcade game console [Start] and [A] buttons have the same behavior.

If you have any questions about this demo package, please email us. xxxxx@iircade.com
-----------------------------------------------------------------------------------

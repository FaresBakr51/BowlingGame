
Virtual Keyboard

Hello, and thanks for purchasing the Virtual Keyboard. I developed this keyboard because I wanted to create games that could be played from install to finish using only a controller. This allows for user input without the need for a physical keyboard.

HOW TO
 - Drag the Keyboard prefab onto a canvas in your scene
    - In order for the keyboard to scale correctly, it should be on a canvas set to Scale With Screen Size
    - For testing, I used a Reference Resolution of 1920 by 1080 and set it to match on the Width
    - Testing with a wide range of resolutions, the keyboard scaled perfectly across all of them

 - Select the VirtualKeyboard object in the Hierarchy

 - Drag the InputField (textbox) you want the keyboard to update from your scene into the "Text Box To Update" field in the Inspector

 - Make sure the "Upper Keys" field is set to the upperKeys object
    - If it is not, expand the VirtualKeyboard object in the Hierarchy and drag upperKeys into the Inspector under "Upper Keys"


CUSTOMIZATION OPTIONS
 - You can change the font

 - You can change the button background images

 - You can change the button color

 - You can modify the button interactions settings

 - You can scale and move the keyboard location to fit your scene

 - You can place multiple keyboards in the same scene to update different input fields


CHANGING THE BUTTON FONT
 1. Open the VirtualKeyboard prefab object

 2. With the prefab object open, search Text in the search bar

 3. Select all Text objects that show from your search results using ctrl + A
 - You can now change the font for all buttons at the same time under the Text module in the Inspector


CHANGING THE BUTTON BACKGROUND IMAGES
 1. Open the VirtualKeyboard prefab object

 2. With the prefab object open, search Btn in the search bar

 3. Select all Btn objects that show from your search results using ctrl + A
   - You can now update the Source Image for the buttons under the Image module in the inspector
   - You can change all buttons, or pick and choose which buttons you want to change the image on


CHANGING THE BUTTON BACKGROUND COLOR
 1. Open the VirtualKeyboard prefab object

 2. With the prefab object open, search Btn in the search bar

 3. Select all Btn objects that show from your search results using ctrl + A
   - You can now update the Color option for the buttons under the Image module in the inspector


SETTING BUTTON INTERACTION BEHAVIOR
 1. Open the VirtualKeyboard prefab object

 2. With the prefab object open, search Btn in the search bar

 3. Select all Btn objects that show from your search results using ctrl + A

 4. Find the Button module in the Inspector
 - You can now adjust the normal, highlighted, pressed, selected and disabled colors for the keyboard buttons
   - Play around with different colors and see what settings work best with your project
 
 * NOTE - Do not modify the Target Graphic for any of the buttons


MOVING AND SCALING THE KEYBOARD
 - Inside the VirtualKeyboard prefab object you placed in your canvas, there is an object named "moveThisForKeyboardPlacement"

 - You can adjust the scale and position of this object to adjust the buttons with it

 - Scale and move this object within your scene to adjust the entire keyboard 


MULTIPLE KEYBOARDS
 - Drag as many keyboard prefabs you need onto your canvas

 - Adjust their size and positions to fit within your scene

 - Update the "Text Box To Update" field for each individual prefab keyboard object to update the current input field

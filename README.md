[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/p3tAls-C)

# FP Earth-Core
[![Alt text](https://github.com/cg20231c/final-project-earth-core/assets/114988957/5d857c46-6043-4983-bca1-895ed6608364)](https://youtu.be/FHpxUJXPSa4)
Gameplay: https://youtu.be/FHpxUJXPSa4

Greetings fellow CG20231c ! we are Earth-Core!  
Team members:
- 5025211139 - Apta Rasendriya Wijaya  
- 5025211160 - Made Nanda Wija Vahindra  
- 5025211084 - Arfi Raushani Fikra  
- 5025211100 - Muhammad Zhafran  

Embark on a captivating journey in Mr.GRAFKOM, an immersive first-person movement game developed in Unity. This dynamic experience goes beyond traditional movement games, introducing a visually stunning world with a living day and night cycle, offering players a unique and evolving adventure. What sets Mr.GRAFKOM apart is its support for both traditional mouse and keyboard controls as well as joystick compatibility, providing players with the freedom to choose their preferred input method.  
  
Features:
### Multiple Control Options:
- Seamlessly navigate the environment with responsive mouse and keyboard controls for precise movements.
- Immerse yourself in the game with joystick support, offering a more intuitive and dynamic control experience.
### Basic Movement:
- Master the art of movement using your preferred input method, whether it's the precision of mouse and keyboard or the analog finesse of a joystick.
- Jump, sprint, crouch, and explore vertical spaces with ease, adapting to your chosen control scheme seamlessly.
### Advanced Movement Mechanics:
- Slide, grapple, climb, and swing with precision, utilizing the full range of your chosen controls for a dynamic and exhilarating experience.
- Enjoy a smooth and immersive traversal whether you're using a joystick or traditional controls.
### Animation and Sound:
- Each movement is accompanied by meticulously crafted animations, enhancing the sense of realism.
- Sound effects provide auditory feedback for every action, creating a multisensory gaming experience tailored to your control preferences.
### Day and Night Cycle:
- Experience a living world with a seamless day and night cycle, influencing gameplay conditions.
### Objective-Based Gameplay:
- Collect coins strategically placed throughout the map in diverse environments.
### Visually Pleasing Environments:
- Explore breathtaking landscapes with meticulous attention to detail.  

Mr.GRAFKOM offers a simple yet engaging experience, giving players the freedom to enjoy the game their way.

## Implementation
### Animation and Sound
Each movement have state where it will we play when it trigger from input player (it already set in [animatorStateController.cs](character/Canimation/animatorStateController.cs)), this is how state connect each other so it can be play with trantition each move.
![image](https://github.com/cg20231c/final-project-earth-core/assets/116022017/b4eb47b4-587e-4dc6-8958-ae5680a26d4a)
to have that animation and character we can use asset from https://www.mixamo.com/#/?page=1&type=Motion%2CMotionPack
Visualization Animation:
<img width="1080" src="Image Readme/animation_visual.gif" />  
### Movement 
Camera  
For the camera movement the logic is simple. When we move the mouse in X axis, the camera and the player should rotate horizontally, when we move the mouse in Y axis the camera should rotate vertically  
https://github.com/cg20231c/final-project-earth-core/blob/fe839a0eab80dd547eac31219b4732611bb21faf/Movement%20Scripts/PlayerCam.cs#L34-L48  
This is the code for the camera movement, i also add joystick input so we can also move the camera using right analog. The script is applied in the camera, and the rotation will applied to the camera and orientation of the player (ignore the isFlipped variable since it is an unfinished feature)  
https://github.com/cg20231c/final-project-earth-core/blob/fe839a0eab80dd547eac31219b4732611bb21faf/Movement%20Scripts/PlayerCam.cs#L7-L13  
The script have public variable which can be defined in the unity inspector  
![image](https://github.com/cg20231c/final-project-earth-core/assets/114988957/b245b4b5-3132-4694-9825-8b9c64448fc3)  
![image](https://github.com/cg20231c/final-project-earth-core/assets/114988957/b2705ebd-e510-43db-b2c8-03d76be4798b)  
for the camera movement, i put the camera in camera holder and add script to the camera holder to follow the position of camerapos in player
https://github.com/cg20231c/final-project-earth-core/blob/fe839a0eab80dd547eac31219b4732611bb21faf/Movement%20Scripts/MoveCamera.cs#L17  
So the camera will follow the movement of the player.  
This is the player movement script  
https://github.com/cg20231c/final-project-earth-core/blob/fe839a0eab80dd547eac31219b4732611bb21faf/Movement%20Scripts/PlayerMovement.cs#L261-L290  
Because i add some advanced movement it becomes a little complicated, but basically if you do grappling or wall jump you can't move the player. The basic movement happen when the player is grounded, it add force to the rigidbody based on the movespeed we set and the move direction.  
https://github.com/cg20231c/final-project-earth-core/blob/fe839a0eab80dd547eac31219b4732611bb21faf/Movement%20Scripts/PlayerMovement.cs#L292-L309  
I also add speed control to limit the movespeed of the player but it ignored in some condition.  
https://github.com/cg20231c/final-project-earth-core/blob/fe839a0eab80dd547eac31219b4732611bb21faf/Movement%20Scripts/PlayerMovement.cs#L167-L232  
There are also a state handler function to control the speed of the player but I also add a condition if the player speed changed over 4f the speed will decrease overtime and not quickly changed so the player can keep their momentum.
The rest of the function can be seen in [PlayerMovementScript](Movement%20Scripts/PlayerMovement.cs).  
Sliding  
This is function for sliding  
https://github.com/cg20231c/final-project-earth-core/blob/4809344a037e11d661bed27d2b6f8bf16984bedb/Movement%20Scripts/Sliding.cs#L73-L92  
if the player is on slope we add force to the rigidbody and start the timer so the player only can slide for only a few second, but if the player is on slope and slide down the slope the player can always perform sliding.  
[SlidingScript](Movement%20Scripts/Sliding.cs)  
Climbing  
To perform climbing, the player must face a wall first
https://github.com/cg20231c/final-project-earth-core/blob/4809344a037e11d661bed27d2b6f8bf16984bedb/Movement%20Scripts/climbing.cs#L115-L127  
The wallcheck function use SphereCast to detect wall in front of the player, and update the climbTimer so the player can perform the climb. We also add several condition, the full script can be seen [ClimingScript](Movement%20Scripts/climbing.cs)  
Grappling  
![image](https://github.com/cg20231c/final-project-earth-core/assets/114988957/3212b886-502a-4ca9-bf2b-c92ddee5ec51)  
https://github.com/cg20231c/final-project-earth-core/blob/4809344a037e11d661bed27d2b6f8bf16984bedb/Movement%20Scripts/Grappling.cs#L83-L97  
The grappling function only calculate the point of the grapple and perform JumpToPosition Function to launch the player to the grapple point, the fucntion can be seen in [PlayerMovementScript](Movement%20Scripts/PlayerMovement.cs).  
For the rope/web we use line renderer.
Swinging  
for the swinging movement we add prediction point to get the rope position and shoot there  
![image](https://github.com/cg20231c/final-project-earth-core/assets/114988957/6a8525c6-2f45-45e9-82e3-aa943c71ae03)  
https://github.com/cg20231c/final-project-earth-core/blob/95e2a0bfc409d0d3522d44bc4997ce112c616cea/Movement%20Scripts/Swinging.cs#L118-L152  
And to swing we use this function applied to the player
https://github.com/cg20231c/final-project-earth-core/blob/95e2a0bfc409d0d3522d44bc4997ce112c616cea/Movement%20Scripts/Swinging.cs#L155-L182  
Sorry but we cannot explain all the codes because it will take a lot of time and resources. We code the scripts manually with some references from youtube, you can see the scripts in the github repository and see the result in the gameplay video on youtube.
### Day and Night Cycle
For the day and night cycle we use script to control the rotation of the directional light
https://github.com/cg20231c/final-project-earth-core/blob/95e2a0bfc409d0d3522d44bc4997ce112c616cea/Day-Night%20Cycle%20Scripts/DayNightCycles.cs#L27-L80  
In the update function we update the city material by calling ChangeMaterial() from [DayNightScript](Day-Night%20Cycle%20Scripts/DayNight.cs) according to the time, and update the sun rotation and the lighting(equator color, sky color, and sun color), [Here is the result](https://youtu.be/FHpxUJXPSa4?si=0zVv2M0rz6GtVLLp&t=84)

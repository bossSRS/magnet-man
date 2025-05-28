# magnet-man

You have to make a small prototype for android device in Unity Game Engine and provide APK to us. The basic concept of prototype is based on Magnetic field. You know there are two poles of a Magnet that North and South pole. Opposite poles (North and South) attract each other and same poles repel. Here North pole will be represented by "Red" color and the South pole by "Blue" color. In your prototype, assume that there are discrete North (Red) and South (blue) pole randomly placed in different positions. And your main Player also acts as either North pole or South pole. Your Player will travel one end to another end through these discrete poles. 
We have provided a demo prototype, basically you have to make a clone of our prototype. Here are the details of it.
1. The player is a simple Capsule with a small face (use a box to represent face). And it can move in any horizontal direction by a simple joystick, can Jump by Swipe up finger on the screen and speed up for a short moment (0.5 second) by quick Double Tap on screen.
2. The player is continuously changing its pole (toggle between North and South) after a few seconds (3 to 5).
3. There are some boxes with Colliders like walls for boundary and like a small platform to stand on it.
4. There are two shaped randomly placed discrete poles, one is Cylinder and another is a Box. Each shape can act both as the North Pole (Red) or South Pole (Blue). The cylinder shaped poles always move in any horizontal direction and Box shaped poles are static. Remember that Box shaped poles are different in their size (Squire, rectangle, big, small) and placed by any rotations.
5. All discrete poles (static or dynamic) attract or repel player by perpendicular direction with their surfaces (not to their center) according to Magnetic Low.
6. There is a Yellow platform at the end of the stage, when the player enters into this platform the Screen will fade out and fade in with Player starting position.
7. The Camera is a simple side view game camera that always follow the player. 
Note: You have to use the default game objects (Capsule, Box, Cylinder, Plane, etc.) for your prototype.      And your prototype has to run smoothly with 60 FPS on average android devices with 2GB Ram.

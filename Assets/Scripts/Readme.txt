Authors: Tempitope Adebanjo, Eeshaan Reddy, Charles Scott
Sample code provided by Leonard Brown

AI onnx files can be found in the Assets/Brains directory. Tensorboard screenshot is in scripts.
The imitation learning model ultimately was unable to complete a lap after 5 million steps of training. Likely the issue lies with my poor demonstrations.
The AI was given 6 episodes of Charles running the track pretty poorly, one of which was a failure at about 3/4 of the way due to backing up into a previous waypoint.
The RL model performs much better, successfully completing laps reliably and with speed.
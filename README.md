# wheel-of-luck
Using the Wheel of Luck Package

Installation:

Import the Wheel of Luck package into your Unity project.


Setup:
a. Create a new GameObject in your scene and add the following components:

WheelOfLuck
WheelSpinner
RewardManager
UIManager
b. Create a WheelConfig ScriptableObject (Right-click in Project window > Create > Wheel of Luck > Wheel Config)
c. Assign the WheelConfig to the WheelOfLuck component in the inspector.


Creating Rewards:
a. Create ConsumableRewards:
Right-click in Project window > Create > Rewards > Consumable
b. Create UniqueRewards:
Right-click in Project window > Create > Rewards > Unique
c. Configure each reward's properties in the inspector.
Configuring the Wheel:
a. Open your WheelConfig asset.
b. Set the total positions, consumable positions, item positions, and other parameters.
c. Assign initial custom rewards if desired.
Setting Up the UI:
a. Create UI elements for the wheel display, spin button, and message text.
b. Assign these elements to the UIManager component in the inspector.
Implementing Game Logic:
a. Use the GameManager to handle game-wide logic and player data.
b. Implement methods to update player currency, items, and other game states when rewards are claimed.
Spinning the Wheel:

Call the Spin() method on the WheelOfLuck component when the player wants to spin the wheel.


Customization:

Modify the WheelSpinner class to change the spinning animation.
Extend the Reward class to create new types of rewards.
Adjust the UIManager to customize the wheel's appearance.


Testing:

Play the scene and test the wheel by clicking the spin button.
Verify that rewards are being distributed correctly and applied to the player's inventory.


Integration:

Integrate the Wheel of Luck with your game's economy system, ensuring proper currency deduction for spins and reward application.



Note: This package provides a framework for a Wheel of Luck system. You may need to implement additional game-specific logic, such as currency management, inventory systems, or persistent data storage, depending on your game's requirements.

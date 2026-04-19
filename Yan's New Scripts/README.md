Quick update on how to use the new scripts:

Stop using: Damage.cs and EnemyFollow.cs (they are now obsolete).

Add these instead: EnemyHealth.cs, ZombieAI.cs, Knockback.cs, and ContactDamage.cs.

Note: I've already modified LevelManager.cs to update references from Damage to EnemyHealth.

hysics Setup:
To ensure enemies collide with walls but still detect hits, you must add two Box Collider 2D components to the enemy:

1st Collider: Smaller, with Is Trigger unchecked (handles physical collisions with walls).

2nd Collider: Slightly larger, with Is Trigger checked (handles hit detection).

Double Damage Note:
Please be aware that having two colliders might cause a "double damage" issue where a single hit counts twice. I have implemented a hit cooldown (I-frames) in EnemyHealth.cs to prevent this, but make sure the "Trigger" collider is the one primarily used for combat detection to keep it clean.
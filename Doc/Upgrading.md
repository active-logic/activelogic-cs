## Upgrading from Active-LT to Active-Logic

First, license the Active Logic package [from the Unity Asset Store](http://u3d.as/1AZ8).

Your upgrade path depends on whether you are using the UPM package (Github) or the UAS Active-LT asset (from the Unity Asset Store).

Before upgrading, backup or commit your project.

### 1. Remove Active-LT

**If using Active-LT via UPM/Github** -
Open your project and display the Unity Package Manager. Find and select Active-LT, then choose "Remove" in the bottom right corner.

**If using Active-lT via the Unity Asset Store** - Open your project and display the project window. Delete the *ActiveLogic* directory.

After removing the package, you will probably see errors in the console; this is normal and these errors may be safely ignored.

### 2. Install Active Logic

From the Unity Asset Store window, download and import the Active Logic Package. In some versions of AL, you may see warnings in the console window; these warnings may be safely ignored.

### 3. Retest your project

Press "Play" to retest your project.

In some cases, your agents may not run. If this happens, you only need to reassign the "root" task in the matching Agent, Ticker or PhysicsAgent.

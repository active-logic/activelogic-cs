# Patches how-to

Patches are used to fix significant issues ahead of Unity asset store updates. This document lists released versions, along with relevant issues.

If you are not using Unity, or only using the lighter integration available on Github, patches are not necessary; keep your repository up to date or re-install the latest \*.pkg.

Currently no automation is planned for patch releases; grab the file containing the patch, and override the matching file in your package install.

## v1.0

[LogTree.cs](v1.0/LogTree.cs) - Fixes two bugs causing delayed refresh of log-tree view; symptom: while running your project (play mode), select a logging object in tree view and observe that the log-tree view does not update.

[History.cs](v1.0/History.cs) - History should be `MonoBehaviour`, not `Behaviour` (otherwise no history)

*Note: if logging is not enabled for a specific game object, or the game object does not use a stepper (one of `Agent`, `PhysicsAgent` or `Stepper`), the log tree view will not change; this is by design and is not affected by this patch*

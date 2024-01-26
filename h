[33mcommit fcca4857d299b45a95f6f51e93e795260b09c846[m[33m ([m[1;36mHEAD -> [m[1;32mdevelop2[m[33m)[m
Author: dragvoll <dragvoll@NTNU00668.win.ntnu.no>
Date:   Fri Jan 26 10:58:25 2024 +0100

    updated boat

[33mcommit b091a116e22e506a5a358f3c18369a519270c89c[m[33m ([m[1;31morigin/develop2[m[33m, [m[1;31morigin/NPCIntegration[m[33m)[m
Author: Magnus <magnus.baugerud@hotmail.com>
Date:   Mon Jan 15 11:49:58 2024 +0100

    add NPC to Fish welfare task

[33mcommit 5377204726924e23f19a55edd30b4ee0a2ed6053[m[33m ([m[1;33mtag: v0.3[m[33m, [m[1;31morigin/develop[m[33m)[m
Merge: 840add5d 773f99df
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Tue Nov 21 12:18:03 2023 +0100

    Merge pull request #231 from vr4vet/getTabletFix2
    
    fix: added Tablet fix 2 from VR4VET

[33mcommit 773f99df756fa161e9a114d4c02ddf5c0654880a[m[33m ([m[1;31morigin/getTabletFix2[m[33m)[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Nov 21 11:45:04 2023 +0100

    fix: added Tablet fix 2 from VR4VET
    
    The skill page now shows the score for each skill. The score matrix in
    the score page is temporarily removed.
    
    Refs: #229

[33mcommit 840add5d4d541e22aba56e04b4eb4ace9cf6b101[m
Merge: a03b77b4 747b9ece
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Tue Nov 14 10:21:03 2023 +0100

    Merge pull request #228 from vr4vet/FishSwimUniformlyFix
    
    Make fish swim less uniformly

[33mcommit 747b9ece5f932f91bf60aef954aa2a9c1b9487e6[m
Merge: 084fd6ea a03b77b4
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Nov 10 13:53:16 2023 +0100

    Merge branch 'develop' of https://github.com/vr4vet/Blue-Sector into FishSwimUniformlyFix

[33mcommit a03b77b4b63357914ce8aa87e8a0921246aec0b6[m
Merge: 8d04de0b 637a2f8f
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Fri Nov 10 13:51:46 2023 +0100

    Merge pull request #226 from vr4vet/AddTablet
    
    Add tablet to FishFeeding

[33mcommit 637a2f8f65af24731c5f9aaad9a09d0366f1ab5c[m[33m ([m[1;31morigin/AddTablet[m[33m)[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Nov 8 14:32:08 2023 +0100

    fix: fixed hologram bug and completion of subtasks in build #178

[33mcommit 102af27d77df8bc9d7c195f8c980ec9482d6dc17[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Nov 7 16:58:09 2023 +0100

    feat: added new Tablet fix and score page is shown
    
    Score page is shown but is still not finished and doesn't show updated
    score values.
    
    Refs: #178

[33mcommit cff7c7ed3e0fa31e6e8d007a55415d230b7c0436[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Nov 7 13:33:11 2023 +0100

    fix: added new material for pointer which renders over iu
    
    Might not need this fix in the future since the pointers were orginally turned off.
    
    Refs: #178

[33mcommit 63cf9bfa1cdddafd60836f42a433b5fec4c8686a[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Nov 7 12:17:41 2023 +0100

    fix: fixed scoreText for build and removed unused files #178

[33mcommit 084fd6ea0ad40c6dbfea7b5e2bd9a6baba1dc104[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Nov 6 15:04:53 2023 +0100

    fix: give rng seed based on instance ID
    
    - FishScript.cs initiates Random seed using its own instance ID.
    - All fish therefore instantiate with unique seeds.
    - Fish no longer turn at the same time.
    - Also made variable controlling hunger/full cycle reset when game is
      over, which should prevent fish from becoming hungry too quickly when
      starting a new round.
    
    Refs: #227

[33mcommit 8d04de0b72c6ff2148f0c9e95dd4df4b567f1952[m
Merge: 6743d2ed d0229a8e
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Mon Nov 6 12:06:34 2023 +0100

    Merge pull request #225 from vr4vet/LevelsSceneTransitionBug
    
    fix: levels can be toggled after fish welfare

[33mcommit 83cb2578509183e1de1988cdbe35ce2b9e9b3a6a[m
Merge: 61bb66fb d0229a8e
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Nov 3 12:18:20 2023 +0100

    Merge branch 'LevelsSceneTransitionBug' of https://github.com/vr4vet/Blue-Sector into AddTablet

[33mcommit 61bb66fb1474e5846ffc8a0b0bb338db7586f964[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Nov 3 12:16:19 2023 +0100

    misc: turned on pointer and removed dummy tasks and subtasks #178

[33mcommit aef7a4955f5a7584a1d66fa15c16d91d83283109[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Nov 3 11:28:23 2023 +0100

    misc: added comments for ScoreTablet.cs

[33mcommit 5ba9c3adf3b6b1e189714f7fbf086616bb7c320e[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 29 17:47:34 2023 +0100

    feat: completed more of the evaluation script for fishfeeding #178

[33mcommit d0229a8e01fe69a5a3004091c24a73ae3a4446b4[m[33m ([m[1;31morigin/LevelsSceneTransitionBug[m[33m)[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sun Oct 29 13:59:25 2023 +0100

    fix: levels can be toggled after fish welfare
    
    - Fixed by removing DontDestroyOnLoad from ModeLoader (I don't know why
      this was there in the first place, everything still works).
    - Also fixed a bug where score didn't appear after finishing a round
      because FishSystemScript addempted to revive objects that are not
      fish.
    
    Refs: #224

[33mcommit 6743d2ed969299db16c32259e6fb5d8f90f71ce4[m
Merge: d4815579 9ef7d904
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Wed Oct 25 11:29:29 2023 +0200

    Merge pull request #223 from vr4vet/ImprovePerformance
    
    Massively improved performance in fish feeding scenario

[33mcommit 931b737775679055ecebe05c86e00d3a6ec8a204[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Oct 24 19:08:14 2023 +0200

    feat: ScoreTablet script gives score to each subtask #178

[33mcommit e6db308967c77e6900a7848b007e9d77ac8cc42b[m
Merge: f8ca7cca 9ef7d904
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Oct 24 17:04:14 2023 +0200

    Merge branch 'ImprovePerformance' of https://github.com/vr4vet/Blue-Sector into AddTablet

[33mcommit 9ef7d904981a2d3b9f03d62441ea2a0f404b3d47[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Oct 24 16:07:11 2023 +0200

    fix: fixed invisible merd in camera
    
    The fishmerd prefab wasn't on the same layer as the camera because the
    change was saved in a prefab that is gitignored. Added the layer for the
    fishmerd prefab and saved it in the scene.
    
    Refs: #222

[33mcommit f8ca7cca043fbf261b2ddbb9c19b03ee574dbd16[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Oct 24 15:35:18 2023 +0200

    feat: added more for ScoreTablet script #178

[33mcommit 139f8288a123ff18fb30857f68d94f759b9fc90a[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Oct 24 12:37:18 2023 +0200

    fix: fixed missing MerdCamera and KillFish bug
    
    Fixed missing MerdCamera error. The changes were saved in the FishMerd
    folder which is gitignored and added MerdCamera to the FishSystem
    prefab. There were also a bug when killing fish and fixed that.
    
    Refs: #222

[33mcommit e3e9ae42129216dd559d5137a38ab79e2a1c156c[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Oct 23 17:09:14 2023 +0200

    fix: replaced ocean in fish welfare with new ocean
    
    - This does not improve performance in welfare scenario, as the
      high-poly fish with physics are still there.
    - Also cleaned up code and added comments for clarity.
    
    Refs: #222

[33mcommit 6a7d2762259837a2ed1f832453c321bbd831141f[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Oct 23 16:31:53 2023 +0200

    fix: remove red house reflections on water
    
    Removed this by setting Reflectoin Probes to off in OceanSurfaceMobile
    prefabs in scene. Watch out however, as this sometimes causes all
    reflections to dissapear. Can be finicky, but reflects normally without
    all the red colours now.
    Also increased fish count to 400 per cage, and increased the fish
    system's radius so fish swim all the way to the edge of the cage.
    Performance is still great, averaging at 72 fps still.
    Cleaned up teleportPlayerToCamera script a little.
    
    Refs: #222

[33mcommit 945b9f0dd6353493b6d7006defa91c210409df53[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Oct 23 15:22:52 2023 +0200

    fix: fish leaving cage bug
    
    Added an extra check so fish don't leave their cages, and removed the
    sprite LoD-level, as it's no longer necessary, and doesn't play well
    with fog.
    
    Refs: #222

[33mcommit 93085325d143e4289f696d600d619a9b9168bd49[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Oct 23 15:00:26 2023 +0200

    feat: recreate underwater effect without shader
    
    Implemended a blue standard unity fog on cage cameras. This looks very
    close to the original shader without compromising performance. The game
    also runs at a good fps with 300 fish per cage (900 total).
    
    Refs: #222

[33mcommit 1eefc3331f26403fe9ae46ef00c0bb7ce8dd1b20[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sun Oct 22 18:26:36 2023 +0200

    feat: replaced shader with traditional techniques
    
    - Scrolling textures on water surface
    - Underwater shader replaced by a transparent plane
    - Replaced speed boat wave bopping with a sine function.
    - Result is massive performance improvement. Went from 17 fps to 72 fps
      (max on Quest 2).
    
    Refs: #222

[33mcommit a9c90a843debf3fedd45b7019b100bef5352125e[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 22 18:16:21 2023 +0200

    fix: fixed bug for checking feeding intensity and click on for tablet #178

[33mcommit 8b35b6d6bc180b635e5b7d4e040c334b3f3c2c71[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sun Oct 22 14:09:06 2023 +0200

    feat: added new experimental ocean surface
    
    This is simply a plane with a scrolling texture. Does not look as good,
    but it might have to be like that when not using post processing.
    
    Refs: #222

[33mcommit 01fef29af9244d5be6e958152aa394bb5225144b[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 22 12:55:33 2023 +0200

    feat: modified subtasks and skills #178

[33mcommit d64ab8d5028b3a21224b7b3238555de0401bfb6e[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Oct 20 14:59:03 2023 +0200

    perf: reduced amount of fish and cleaned code
    
    Lower amount of fish and removed prevented some variables from getting
    set over and over. Performance is still not ideal.
    
    Refs: #222

[33mcommit 1df20ca1493ad7a5d5f0d39df44daa99b93d8bd4[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Oct 20 13:46:55 2023 +0200

    perf: code, camera, culling, bloom
    
    Made fish start co-routines at random offsets so they don't all do them
    simultaneously. Also made cage camera not render the house, and limited
    it's rendering distance. Also disabled bloom.
    
    Refs: #222

[33mcommit d4815579b4c4c87bf738f5ebad0b6a5aca4508a7[m
Merge: 57bf96c1 a5481c96
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Fri Oct 20 11:57:48 2023 +0200

    Merge pull request #220 from vr4vet/FishRenderingFix
    
    Fish rendering and camera track fix

[33mcommit a5481c9658094694ad30b305454461a542b522dd[m
Merge: 14f31805 57bf96c1
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Oct 20 11:52:07 2023 +0200

    Merge branch 'develop' of https://github.com/vr4vet/Blue-Sector into FishRenderingFix

[33mcommit 57bf96c12aea0f6d512ebcf2a2e30fdcf1ec4a59[m
Merge: 8f9be63a dafed9ee
Author: Snorre <114170492+SnorreForbregd@users.noreply.github.com>
Date:   Wed Oct 18 09:37:49 2023 +0200

    Merge pull request #219 from vr4vet/LevelsErrorFix
    
    Changed from Start to Awake in Levels.cs

[33mcommit 14f31805100c3ebe1c7019e664dd36b0d26d440f[m
Merge: 9ce46638 8f9be63a
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Oct 17 12:16:24 2023 +0200

    Merge branch 'develop' of https://github.com/vr4vet/Blue-Sector into FishRenderingFix

[33mcommit dafed9eeba99b03f246674ef66af188185fb045d[m
Author: Snorrfo <snorre.forbregd@ntnu.no>
Date:   Mon Oct 16 10:20:29 2023 +0200

    Changed from Start to Awake in Levels.cs

[33mcommit 7a5139d654c18d269464cbf040443f919dd9b2ff[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 15 19:04:50 2023 +0200

    feat: created ScoreTablet script which completes subtasks when triggered #178

[33mcommit 25ebac12214e734d7ca1c8254201643cd941c755[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 15 15:14:32 2023 +0200

    fix: add subtasks to tasks and updated tablet with release #177

[33mcommit 9ce46638e3222bc92b842a6b358750b35932f817[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Oct 13 16:16:37 2023 +0200

    fix: hologram cam movement
    
    Divided height by two, which fixed the issue. I chose to keep the
    attributes for the user for amplifying Y and Z movement, as different
    cage systmes will likely require that flexibility.
    
    Refs: #205

[33mcommit 56a2fe90c47b04928ac6e0e62124b6749ccac29f[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Oct 13 15:59:00 2023 +0200

    fix: repair hologram and increase fish culling
    
    The previous fixes broke the fish hologram. This is a workaround by
    giving the user the option to increase/decrease the Z and Y-axis
    speed/distance of the hologram camera.
    Also increased the culling distance of fish, preventing them from
    dissapearing. The main bug of dissapearing fish should now be fixed.
    
    Refs: #205

[33mcommit 4ee08e1bae4e9cabc19f468acb211a7c77e7932e[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Oct 13 15:26:37 2023 +0200

    fix: did it properly
    
    Made MerdCamera a child of CameraTrack, which fixes the problem. Also
    attempted to draw the camera track in Unity GUI, replacing the useless
    collider used for that purpose. It reacts to scaling etc, but is
    probably not completely accurate as the math involved in MerdCamera is
    confusing.
    Lastly adjusted the scale of camera track to prevent camera from going
    above water surface.
    
    Refs: #205

[33mcommit a5043d5a6958584ea62b09897ebb5dd74e3efcd3[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Oct 13 12:58:49 2023 +0200

    fix: preventing camera from leaving cage
    
    Kind of an hoc solution by referencing the fish system's position
    instead of the camera track. The solution is really to make the
    MerdCamera a child of its camera track, rather than the other way
    around, but this fix works.
    
    Refs: #205

[33mcommit c29f22ea822d68f43bd0bddc3a10cd4470ea192a[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Thu Oct 12 15:57:01 2023 +0200

    fix: fish LoD and button/hologram bloom
    
    Enabled HDR, which enables bloom. Might have consequences for
    performance, but is not noticable at the moment as there are other
    factors (probably the fish system for the most part) affecting
    performance more.
    Also adjusted color of fish sprite to make the transition from 3D-model
    to sprite less jarring. This might also mitigate the problem where fish
    "dissapear" when looking up from the bottom of a cage.
    Increased intensity of underwater shader, making it look closer to
    PC/Unity.
    
    Refs: #205

[33mcommit 8f9be63a18bb51298ea20dfc18101144fb2ed5de[m
Merge: 89f36184 4dcdfa57
Author: Frequin <91453915+ingolvjr@users.noreply.github.com>
Date:   Thu Oct 12 12:07:16 2023 +0200

    Merge pull request #218 from vr4vet/SceneTransition
    
    Scene transition script from VR4VET

[33mcommit 89f361845f8621908b001a246c448a18d893b7e5[m
Merge: 64e448dc 7a57f2f3
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Thu Oct 12 11:24:13 2023 +0200

    Merge pull request #217 from vr4vet/ImmersiveViewShaderFix
    
    Immersive view shader fix

[33mcommit 4dcdfa57d207b8f8b13e1ebe5d9c09390f62fafc[m
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Thu Oct 12 09:40:09 2023 +0200

    added the sceneTransition script to the scenes

[33mcommit 7a57f2f359a95c2b70e284e862f4c2e2b8e124f9[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Oct 9 15:35:48 2023 +0200

    fix: nauseating underwater bug
    
    Was caused by unreferenced shader files. Not sure exactly which ones are
    necessary, as building for Android tends to worsen/break shaders, making it
    hard to see if effects are enabled properly or not. However, it is safe
    to say that the one named "underwater" is. There is now a blue underwater hue and fog (probably nothing new), and the choppy effects on the left eye is gone. Still looks kinda bad because Android (I think), and the performance is, as already known, not very good.
    
    Refs: #216

[33mcommit 37e3a93e71bab7ace8fd865fd442572e59ed5ba7[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 8 16:42:30 2023 +0200

    misc: modified tasks, skills and subtasks #177

[33mcommit 2984ece3101006ddbf4b206c488f8dbde3b09145[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 8 16:40:57 2023 +0200

    fix: removed previous errors and added tasks, skills and subtasks #177

[33mcommit b0e7332bccb2766298bca99cc32139850848c769[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 8 14:57:54 2023 +0200

    misc: added new and updated files for tablet and tasks from vr4vet
    
    There are errors from a script in FishWelfare because it uses an old
    method that no longer exists in TaskHolder. Other errors comes from
    files who are also old and aren't used anymore.
    
    Refs: #177

[33mcommit 80ddc5d92a71640a14c5e0b17f7c814bbace4663[m[33m ([m[1;33mtag: v0.2[m[33m, [m[1;31morigin/main[m[33m, [m[1;31morigin/HEAD[m[33m, [m[1;32mmain[m[33m)[m
Merge: 68a3c178 64e448dc
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Oct 5 14:33:17 2023 +0200

    Merge pull request #215 from vr4vet/develop
    
    Merge working build

[33mcommit 64e448dc820ee67df9b02eca2c3e1f9631bff20a[m
Merge: 6e55fad6 5102d6da
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Tue Oct 3 12:52:30 2023 +0200

    Merge pull request #213 from vr4vet/UITextFix
    
    fixes: Level selector, Immersive view, Start button, UI text

[33mcommit 5102d6da78422b36f3d34bb5878ad03dae507762[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 1 18:06:59 2023 +0200

    fix: updated occlusion data

[33mcommit 4f9a38a6fa0e528e514f5282d58c58e12d86ca0b[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 1 17:51:25 2023 +0200

    fix: changed from TextMeshPro-Text to TextMeshPro-Text(UI) on signs #212

[33mcommit 251b97a15dd3f0856965c86c9301cb5d829148da[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Oct 1 15:03:08 2023 +0200

    fix: updated version of Tutorial.cs without ArrayUtility
    
    Co-authored-by: Snorrfo <snorre.forbregd@ntnu.no>

[33mcommit 6c015c812e81857518a36654eb174c2cde33db7b[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sun Oct 1 14:35:01 2023 +0200

    fix: removed y-axis offset
    
    This makes the player not fall when teleporting back to the office.
    However, just like teleportation otherwise, the player will fall through
    the floor when playing in Unity and not in a build.
    Teleportation now works as intended.
    
    Refs: #209

[33mcommit 07b3878247a11128580a76bc9c67048f4bc63faa[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sun Oct 1 14:08:01 2023 +0200

    misc: cleaned up code
    
    Refs: #209

[33mcommit 47a680c88bef528e421608b485ca26f3a89b88b0[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sun Oct 1 13:12:35 2023 +0200

    fix: using VRIF's teleport function
    
    Hopefully this works.
    
    Refs: #209

[33mcommit 343f73fd15235aa046a4de7b3a7fbcb476a07767[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sun Oct 1 11:34:51 2023 +0200

    fix: referencing player without playerlocator
    
    Trying this to see if it works in a build.
    
    Refs: #209

[33mcommit dbb775e3afa0d30cc3168794367233e2ffe859c8[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sat Sep 30 14:59:21 2023 +0200

    misc: removed unused code
    
    Was able to pick level in build, and start game using A-button. Seems
    like Lan's previous fix actually works, but was bottlenecked by
    tutorials now working properly.
    
    Refs: #210

[33mcommit 18fc8be54125be5df1a1ac23f85a0c1efa55ba1a[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Sat Sep 30 14:32:25 2023 +0200

    Fix: Trying to access xml content as a memory stream.
    
    Refs: #210

[33mcommit 3d2216d7bbcbdf9f95b8fcaddf74cb684d56f9f7[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Sep 29 16:27:33 2023 +0200

    fix: level select and tutorial popups.
    
    Changed location and path for loading XML file, as the Assets folder and
    so on does not exist in a build. Need to make a build to test if it
    works.
    Fixed the tutorial popups not behaving properly in advanced mode by
    adding a loop that disables all tutorial entries in tutorial.cs
    
    Refs: #210

[33mcommit 6e55fad6da0d1d7cd7c179fe7bb9247fd3500421[m
Merge: e0a079a4 415e42f2
Author: Frequin <91453915+ingolvjr@users.noreply.github.com>
Date:   Tue Sep 19 11:38:26 2023 +0200

    Merge pull request #204 from vr4vet/StairImprovement
    
    Added some colliders to the stairs so the player does not have to "walk" up the stairs

[33mcommit 415e42f259579471c5d0c5d6a776a34a21e6c645[m[33m ([m[1;31morigin/StairImprovement[m[33m)[m
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Tue Sep 19 10:39:01 2023 +0200

    now it works and the area should be more accurate

[33mcommit c9d540fa3b55d4a8f05f3f5fe2d1e52060d83b33[m
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Mon Sep 18 12:15:40 2023 +0200

    Trying to use the stair collider in the project, but it doesn't collide with the player

[33mcommit ef5aad62ba5441c00cca4f0efbf95ec16df85f38[m
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Tue Sep 5 10:08:53 2023 +0200

    some of my changes from the last commit disappeared(don't know how/why)

[33mcommit 73df687da4a28d9a324318d90c2758937c7c11fe[m
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Tue Sep 5 09:18:52 2023 +0200

    added colliders to the stairs to make the player instantly move to the top or bottom of the stairs

[33mcommit e0a079a43cdb73f8c0d360345d4dad6b1805f68a[m
Merge: e05a6781 8c1aa3c3
Author: Frequin <91453915+ingolvjr@users.noreply.github.com>
Date:   Mon Aug 21 14:12:18 2023 +0200

    Merge pull request #197 from vr4vet/MerdButtonsAndHintFix
    
    Fixed all issues related to Milestone 07: AquaNor

[33mcommit 8c1aa3c30615e992f9546a096a87c93280b475ac[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Aug 21 12:21:21 2023 +0200

    feat/fix: add waves fix again
    
    Added speedboat script to the speedboats again, and made
    speedboats and their children not static to allow bobbing on water
    surface. Lowered a constant in script to avoid periodic clipping of water through floor.
    
    Refs: #193

[33mcommit c622db9633ef1616947475d0c73a49b40688a11b[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Aug 21 11:57:48 2023 +0200

    fix: fixed missing changes for snapping after merge conflicts #196

[33mcommit 49df81b09c0e450798f1d02ba747c3ee89aa58e6[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Aug 21 11:37:14 2023 +0200

    fix: fixed immersiveview teleportation bug

[33mcommit 62964e398fc4a84da1b2b8b1ff3c546106849174[m
Merge: 100cb4c6 b73a480c
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Mon Aug 21 10:57:54 2023 +0200

    Merge remote-tracking branch 'origin/WaterWavesReduction' into MerdButtonsAndHintFix

[33mcommit 100cb4c6d42f57db4cf1ba792510b125ccede33e[m
Merge: 348413fb 3b5106c3
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Mon Aug 21 10:39:20 2023 +0200

    Merge remote-tracking branch 'origin/JoystickLeversSliderInPockets' into MerdButtonsAndHintFix

[33mcommit 348413fb706b614d2ecc69143d8d6986c332ea1b[m
Author: Frederik Friquin <Frederik.friuin@ntnu.no>
Date:   Mon Aug 21 10:24:06 2023 +0200

    Added the changes from WindowFix branch

[33mcommit b73a480c9a612d5a734ecef037522371f0abf922[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Aug 18 11:31:44 2023 +0200

    fix: boat moves on water surface
    
    The boat does not move exactly like the passing waves, but it is
    generally not noticable when the waves are this mild. The math is
    complex, so doing this correctly is beyond my skills at the moment. The
    illusion is convincing enough as is in my opinion.
    
    Refs: #193

[33mcommit ec82b92a4ab2deac1ca7cc2e8bb2595008cdcc10[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Aug 18 10:32:30 2023 +0200

    Fixed TaskRecapHint to be shown OnRelease instead OnGrip #197

[33mcommit 3b5106c319955471d171bfa9523b70ab121dce38[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Aug 18 08:56:22 2023 +0200

    fix: made objects unstuck in left holster in FishFeeding #182

[33mcommit 7ce066ce2559da1bdc1bdedb623f2a38dea72a83[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Aug 18 08:54:57 2023 +0200

    misc: experimenting with making boat float
    
    Made speed boat float on the ocean, but something is wrong with
    calculating the sinus value. Seems to offset incorrectly. Not complete
    yet.
    
    Refs: #193

[33mcommit a7a054b467ef0eece87981b9b62160927a799d39[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Aug 17 17:07:22 2023 +0200

    Fish and other objects don't get stuck on the left holster #182

[33mcommit 9505d51abe6c914220ffc7f6702bbad433cfd5d2[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Aug 17 15:38:27 2023 +0200

    Turned of snapping to snap zone for joystick, levers and sliders #196

[33mcommit 6a034a9e8fe74469595ae6027cfc9a8e564bcea8[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Thu Aug 17 16:01:12 2023 +0200

    fix: waves no longer clip through floors in boats
    
    Fixed by greatly reducing the amplitude parameter of the Wave structs.
    Water looks more static as a result, but still produces subtle waves.
    
    Refs: #193

[33mcommit 17247a0065d9e03fc82c9ddf278736e1186b3aee[m
Author: Snorrfo <snorre.forbregd@ntnu.no>
Date:   Thu Aug 17 15:12:49 2023 +0200

    Removed Delay from hint tooltips

[33mcommit dd1f85b002a3d4c2af8c286389a46aa7697a0b74[m
Author: Snorrfo <snorre.forbregd@ntnu.no>
Date:   Tue Aug 15 12:58:39 2023 +0200

    Some settings didn't save, this commit fixes that

[33mcommit 6c6d5ec4589348a957f153c64969802747d3c3dc[m
Author: Snorrfo <snorre.forbregd@ntnu.no>
Date:   Tue Aug 15 12:49:56 2023 +0200

    Removed Grabbable Script, Fixed tooltip script ref

[33mcommit e05a6781954891e7a85c495e4f5d840249df6bd5[m[33m ([m[1;33mtag: v0.1[m[33m)[m
Merge: b1f0679f 3faa563a
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Tue Jul 11 11:23:07 2023 +0200

    Merge: Fish feed (#184)
    
    Updates and changes made as part of the course IT2901 in the spring semester of 2023, as well as summer hires for June/July 2023.
    
    Refs: #184
    
    Co-authored-by: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
    Co-authored-by: Trym Lund Flogard <trym@flogard.no>
    Co-authored-by: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
    Co-authored-by: qlpham231 <69513958+qlpham231@users.noreply.github.com>
    Co-authored-by: eamathie <69533149+eamathie@users.noreply.github.com>
    Co-authored-by: TrymPet <trym2001@hotmail.com>
    Co-authored-by: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
    Co-authored-by: asd <dragvoll@NTNU00667.win.ntnu.no>
    Co-authored-by: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>

[33mcommit 3faa563af42ffe49b9e53add270b701326db53f6[m[33m ([m[1;31morigin/fish-feed[m[33m)[m
Merge: 77bf04f0 313c786c
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Tue Jul 4 19:45:20 2023 +0200

    Merge pull request #180 from vr4vet/model-windows-for-building
    
    Model windows for building

[33mcommit 313c786c549abf5dfabb34eb27c110461554e2f5[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jul 4 19:16:08 2023 +0200

    feat: baked fish feed and welfare
    
    Baked new normal and roughness maps after rotating wall texture.
    Baked lighting in Unity multiple times.
    Some materials need more work, as they do not look right.
    
    Co-authored-by: qlpham231 <quynhlanp@gmail.com>
    Refs: #164

[33mcommit 51e89ec50dc8d87841d2f62d98e279cf5607c185[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jul 4 15:41:48 2023 +0200

    fix: metall and glass
    
    Fixed this by baking the diffuse texture again and using Unity materials
    for glass.
    
    Refs: #164

[33mcommit a716185087aa5cf338e5524d72eb4c8f033dfe78[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jul 4 14:37:06 2023 +0200

    fix: merge conflict
    
    Pulled the newest from fish-feed, and reimported the new house prefab
    
    Refs: #164

[33mcommit 77bf04f02a5bc4c5a0a73a7c4fc6b8fe815278d0[m
Merge: 5df0b3c9 bc1b1a04
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Tue Jul 4 12:15:10 2023 +0200

    Merge pull request #179 from vr4vet/add-translation-system
    
    Add translation system

[33mcommit bc1b1a04a241aaf013ae136bc1bcd72d51729f62[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jul 3 16:47:42 2023 +0200

    feat: added signboard for going to fishwelfare and translation #166

[33mcommit 614ab6823b2956a6af33a583c2019dd2c934d4fe[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jul 3 15:52:12 2023 +0200

    feat: added more translation for german and dutch
    
    Added more translations for german and dutch using google translate
    temporarily.
    
    Refs: #166

[33mcommit c534ee824e8c0ccbc4d4dea1844e849183ee76cb[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jul 3 15:39:02 2023 +0200

    misc: added bloom for sirens and removed unecessary file

[33mcommit adf68a5fc969af7d92dfa5d0a0319e3b3afcd19c[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jul 3 11:24:54 2023 +0200

    misc: changed name to string table and asset table #166

[33mcommit 68a899926fe10c1e0a72597e22a68d7976785356[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jul 3 00:10:33 2023 +0200

    feat: localization for current level-text, translates based on current level #166

[33mcommit 9cc8e5cbaa13ca89fb09ad6f879b7dff4544e505[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Jul 2 16:33:30 2023 +0200

    chore: set more stuff to static and scaled lamps better

[33mcommit dc692d21e3038c52359e643d295a2692486829ea[m
Merge: a2b0bab5 5df0b3c9
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Jul 2 16:03:24 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into add-translation-system

[33mcommit 5df0b3c923d510d531e8312f4dba9f758c773b99[m
Merge: f13ac4c9 955033eb
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Sun Jul 2 14:15:59 2023 +0200

    Merge pull request #175 from vr4vet/combine-fishfeed-and-fishwelfare
    
    Combine fishfeed and fishwelfare

[33mcommit 955033eb1107d033f1bd4d867baa764ea80edea8[m
Merge: 4c2062d4 f13ac4c9
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Jul 2 14:12:12 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into combine-fishfeed-and-fishwelfare

[33mcommit f13ac4c917fa6918eaa2323cad004ec04a229542[m
Merge: 91e253a8 948b7a88
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Sun Jul 2 13:36:14 2023 +0200

    Merge pull request #174 from vr4vet/model-jacket-and-boots
    
    Model jacket and boots

[33mcommit 4c2062d4d41cee276cdce7361ab455bd7718a736[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Jun 30 13:54:50 2023 +0200

    chore: increased range of reflection probe to boat and baked

[33mcommit 28c04d5d4362856ce8a62225ecb68537afc2f932[m
Merge: a4d7b8c5 91e253a8
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Jun 30 13:47:24 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into combine-fishfeed-and-fishwelfare

[33mcommit a4d7b8c5929bd7bccd6fcb7e2add27e27db92218[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Jun 30 13:44:58 2023 +0200

    chore: baked lightning in fishwelfare and made more objects to static

[33mcommit 41fa9b3ce6d0f9b40e2472a7faf3a0779af78206[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Jun 30 13:05:16 2023 +0200

    chore: set objects to static where it is necessary

[33mcommit 93f2d1a15966dca99ee7b392ca5797ebf1d6e6ee[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Jun 30 12:36:17 2023 +0200

    feat: welfare and feeding scenes have motorboat sounds for transitions
    
    Cut the length of the motorboat sound and added the sound to welfare
    scene.
    
    Refs: #163

[33mcommit 948b7a885ad4440c39b8bff403329115b77458bf[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Jun 30 12:20:20 2023 +0200

    feat: made hanging jacket, and placed items
    
    Also added a reflection probe in the scene for metallic materials on
    jacket
    
    Refs: #170

[33mcommit 09f2b9e4b83eeb036951dccdf6f9bbc42842bad3[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 29 16:34:21 2023 +0200

    feat; added motorboat driving sounds when tranisitioning scene
    
    Added motorboat driving sound which plays after stepping on the motor
    boat. Still needs to add this to FishWelfare.
    
    Refs: #163

[33mcommit 868b7cb313b5225f1b9a93e6fb9bcbed14285453[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Thu Jun 29 16:30:11 2023 +0200

    feat: make jacket and boot
    
    Modelled and textured jacket and boot using Blender. Works and looks
    good, but need to figure out how to do cloth simulation using joined
    models in Blender to make the jacket hang realisticly
    
    Refs: #170

[33mcommit 91e253a80bf5cf763568e2385e47ae8b6acf3250[m
Merge: d6d3ecd1 4ec48279
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Thu Jun 29 15:07:18 2023 +0200

    Merge pull request #173 from vr4vet/model-siren
    
    Model siren

[33mcommit de7be2dfa6966ebe1dca41c78699e46457ae193f[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 29 14:37:30 2023 +0200

    feat: made FishWelfare look like FishFeed and connected with transition
    
    Added the stuff from the EmptyScene so FishWelfare looks like FishFeed
    on the outside. Added a free motorboat from the unity asset store and
    connected the two scenes with a scene transition triggered on the
    motorboat. Also added the starterTutorial in the FishFeed scene where
    they learn the basic controls.
    
    Refs: #162

[33mcommit 4ec48279159df2dbd0cf381bc6da09b31a6baced[m
Merge: 8be7fb74 d6d3ecd1
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Thu Jun 29 09:39:42 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into model-siren

[33mcommit 63f18ac8a965e5daae9f0798b064441925a1b014[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 28 16:45:19 2023 +0200

    feat: integrated FishWelfare into the same project as FishFeed
    
    Got the newest changes in the FishWelfare folder from the ragdoll
    branch and set up the new tutorial system in FishWelfare. Also fixed
    other bugs that appeared. Modified the startingTutorial area to include physical buttons.
    
    Co-authored-by: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
    Refs: #162

[33mcommit 19554090594f7e87bfa50242f87eb2ace79ce209[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 28 09:58:22 2023 +0200

    misc: removed unecessary mugs

[33mcommit a2b0bab5ba523ee6d54aeab8936c99231e86398d[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 28 09:23:15 2023 +0200

    feat: more translation #166

[33mcommit d6d3ecd129bc45d01d6ba7e6868c41cddf94b3a6[m
Merge: 5742869f a5b748a6
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Wed Jun 28 09:15:50 2023 +0200

    Merge pull request #169 from vr4vet/cutscene
    
    Cutscene

[33mcommit 8be7fb74028d41fc1e7a33229a261570029aa547[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Jun 27 12:36:18 2023 +0200

    feat: 3D model sirens
    
    Modelled, textured, and imported to project. Is now part of the complete
    Unified-Controls prefab
    
    Refs: #172

[33mcommit 5742869fddf38e8d64d93b00e54d1f235507b28f[m
Merge: 4ea852ea fbb554f0
Author: Emil Aron Andresen Mathiesen <69533149+eamathie@users.noreply.github.com>
Date:   Tue Jun 27 12:09:13 2023 +0200

    Merge pull request #168 from vr4vet/vr4vet-tutorial-system
    
    Vr4vet tutorial system

[33mcommit ca73538aa40693d6959665cac35e3e64a8621b53[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 26 15:24:59 2023 +0200

    feat: added more translation for levels and changed table name #166

[33mcommit ca3ba3aba845b5f71f57fbf31e1871659befb86b[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 26 14:12:35 2023 +0200

    feat: added translation for signs in norwegian and english #166

[33mcommit fbb554f0ee23302c9b443936f657ac016925e61f[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 26 13:10:27 2023 +0200

    fix: set box colliders to "Ignore Raycast" layer
    
    Refs: #151

[33mcommit 57245a5e2c08a88ce96cb2785c8bc873e08dc393[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 26 13:09:19 2023 +0200

    feat: changed color of glow material to cage buttons

[33mcommit 83194dbf9f1d444f6f0b53ed472b5870a71252f2[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 26 12:40:28 2023 +0200

    fix: player height
    
    Increased player height because the scene is probably scaled up too much
    
    Refs: #151

[33mcommit 375142e791fef67d8f241e41e98ed8ba4943f8a4[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 26 12:01:03 2023 +0200

    fix: enable bloom
    
    Set XR Rig Advanced VR4VET "HDR" field to "Use Graphics Settings"
    instead of "off"
    
    Refs: #151

[33mcommit f07a06ad8d629b41ff52a66ac2071950b3c5418a[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 26 11:52:25 2023 +0200

    fix: made font white
    
    Refs: #151

[33mcommit 4aa44403786dbc7e399038355bf79251f1e515e9[m
Merge: 1ecfb862 4ea852ea
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 26 11:02:54 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into vr4vet-tutorial-system

[33mcommit 1ecfb86212570f5aff90cbe50ad46d3aa60863f1[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 26 10:58:50 2023 +0200

    fix: lock player position and enable effects
    
    Fixed the issue of the player falling when in immersive view by
    disabling player gravity. Also added a Post-process Layer to the
    player's camera, making the underwater shader appear.
    
    Refs: #151

[33mcommit 44c18a90ab597c2591eaf01429e0154ccdc5abab[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Thu Jun 22 16:17:04 2023 +0200

    fix: fix teleport back from merd
    
    Added a missing line of code to reset player position
    
    Refs: #151

[33mcommit e73ebabc4c19963df213ca73974b8ba7a190b6b7[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 22 16:06:08 2023 +0200

    feat: added translation package and translation for welcome sign #166

[33mcommit f07d6b270cc547e1e382fcf6db1262a3c139e2a9[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Thu Jun 22 15:56:47 2023 +0200

    feat: fixed popup hints
    
    Also switched to the XR Rig Advanced VR4VET. There are still bugs that
    must be fixed related to Immersive View.
    
    Refs: #151

[33mcommit a5b748a60705ebd03dd1dd163c231a172d63d8da[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 22 12:33:15 2023 +0200

    chore: adjusted position to ocean sound in EmptyScene #163

[33mcommit 0f92471c08a174877bf8ac433cf2e5f1775b0c0e[m
Merge: c33bfb6c 4ea852ea
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 22 12:29:52 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into cutscene

[33mcommit c33bfb6c3e33031f250f370844f261e4c7786474[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 22 12:27:33 2023 +0200

    feat: modified EmptyScene to look like FishFeeding on the outside #163

[33mcommit 4ea852ea10044d427fec4ee9a19da383e61f6edd[m
Merge: 0f8fa90b 75a9c99e
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Jun 22 10:35:32 2023 +0200

    Merge pull request #165 from vr4vet/modify-volume-manager
    
    feat: created FishFeedingVolumeManager with more music sources #161

[33mcommit d9af6a63437ca28d7d0ebb75894ad8fca5ddb6a7[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 21 14:56:50 2023 +0200

    feat: created TransitionAoeTrigger prefab to transition scene #163

[33mcommit 1a7e55037b98042df64c2a6d89f1d3b8db4ea547[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Wed Jun 21 14:52:53 2023 +0200

    misc: saved scene
    
    Refs: #151

[33mcommit 7ec67513c131455d9a5003e9e8bcba86df4cb47d[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Wed Jun 21 14:51:14 2023 +0200

    fix: made start game tip appear
    
    There are still problems which have to be solved in Game script
    
    Refs: #151

[33mcommit 627894324da694978367fb9475e3fa89d38f2cf0[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Wed Jun 21 12:31:08 2023 +0200

    fix: ensure that OnCompleted is invoked.
    
    MoveNext now checks if IndexOfCurrentItem is -1, not the length of the
    Items array, as that will never be the case
    
    Refs: #151

[33mcommit 75a9c99ea01605b7c719301dc3dd4e4c1435c357[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Jun 20 16:25:05 2023 +0200

    feat: created FishFeedingVolumeManager with more music sources #161

[33mcommit 3711711675329ab8860b702bcde09a0e246746dd[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jun 20 16:07:14 2023 +0200

    misc: saved changes in scene
    
    Refs: #151

[33mcommit 5b5f75e106fbbfc7a7386877c9d0df271f7cbc8a[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jun 20 15:59:42 2023 +0200

    feat: replaced tutorial system with vr4vet's
    
    Fixed a few bugs in the vr4vet prefab, and replaced the current tutorial
    system with the new one from vr4vet. Seems to work as intended.
    
    Refs: #151

[33mcommit 04f2ef9502e385f348d3430a350fb9b2e5c5ae51[m
Merge: 55ba31bc 0f8fa90b
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jun 20 13:48:00 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into vr4vet-tutorial-system

[33mcommit 0f8fa90b8f887894b88e0950a27a9fac2c2012e8[m
Merge: c450f262 9171423e
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Tue Jun 20 11:36:27 2023 +0200

    Merge pull request #160 from vr4vet/more-realistic-landscape
    
    More realistic landscape

[33mcommit 9171423e5d1de848e4282827b9b4267c8717530e[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Jun 20 11:26:34 2023 +0200

    chore: removed unused files at the root of the Assets folder

[33mcommit 134c76c331a898b90c97ebbc0f40510b465779ea[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 19 14:54:32 2023 +0200

    chore: added reflection probe to fishfeeding scene from kitchenbench

[33mcommit c603bbef6468e7c32311c63f37cb9a114dbdf0cc[m
Merge: 87ca2d86 c450f262
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 19 14:43:48 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into more-realistic-landscape

[33mcommit 87ca2d861c17ee4fbad98f18aa66f71aa76d1d6f[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 19 14:35:49 2023 +0200

    fix: fixed more of the mountains to make them more realistic #158

[33mcommit c450f262eb75121e401f42844b70b36fb0abd8f6[m
Merge: 4675f6fc 0b7e3139
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Mon Jun 19 14:14:38 2023 +0200

    Merge pull request #159 from vr4vet/kitchen-sink
    
    Kitchen sink

[33mcommit f208f1639a85c69426c8a2dc97e1402b52424410[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 19 13:48:48 2023 +0200

    fix: added skybox material to menu which solves previous bug #139

[33mcommit e9aaf6225263e1f4a1634ca86863c4904cfe7577[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 19 13:30:27 2023 +0200

    feat: added more mountains and modified them #158

[33mcommit 0b7e313953ce237b45fcca3e698b78fdeb5ad478[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 19 13:08:10 2023 +0200

    misc: add mugs to office_furniture prefab
    
    Refs: #156

[33mcommit db62fc771bc33dd78171229d0b1db4b6fcd105bc[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 19 13:04:33 2023 +0200

    fix: added reflection probe to the scene, and fixed scale
    
    Metallic materials need reflection probes to "know" what to reflect,
    otherwise they will simply be black. Also, the sink's tap was too small.
    
    Refs: #156

[33mcommit 374db18ffd2dc4e9e8a7303e7df424682f52d059[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 19 11:15:59 2023 +0200

    fix: changed one of the mountains #158

[33mcommit 44e3c415ddbbdf38ac4d0940e3a1cee991ae1c25[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 19 11:00:59 2023 +0200

    feat: added mountains around the house #158

[33mcommit 4bf91bb3d6954ae2c07740c3a62cd4e0bf33f105[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Jun 18 16:37:20 2023 +0200

    fix: made edge of coffee machine sharper
    
    Refs: #156

[33mcommit dc30e02ab5ce703867516e088ccf44a75ef5b999[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Jun 18 16:21:44 2023 +0200

    feat: add kitchen sink and coffee machine
    
    3D modelled and texturer sink/bench and coffee machine, and placed them
    into a scene.
    
    Refs: #156

[33mcommit fdbe8fda9601d675bb29aa627d2f74bbf4c6fde2[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Jun 18 13:39:15 2023 +0200

    feat: changed skybox material to include clouds #158

[33mcommit 4675f6fc3fe03e6e8218879c6b3aa3f82696fce6[m
Merge: cc8ba943 8bc5a962
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Sun Jun 18 12:30:05 2023 +0200

    Merge pull request #157 from vr4vet/sound-instructions
    
    feat: added sound instructions to the signboards

[33mcommit 8bc5a9620ae79867eb2f6f6ff1852001977906c7[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Jun 16 16:10:15 2023 +0200

    feat: added sound instructions to the signboards
    
    Added audio files for three signs. Created AudioAoeTrigger-prefab and
    other scripts to turn on and off audio based on a box collider.
    
    Refs: #135

[33mcommit 55ba31bc3967356c7c654b190116a2205d048ccb[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Jun 16 12:51:17 2023 +0200

    feat: implement and setup vr4vet tutorial system
    
    Not finished, does not fit this scenario well, and I think I should
    spend time on other issues for now
    
    Refs: #151

[33mcommit 20ccced5e6f3b6d956bb3d2cb71a9fe83ded7f33[m
Merge: d90d7e09 cc8ba943
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Fri Jun 16 09:47:02 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into vr4vet-tutorial-system

[33mcommit cc8ba9433bffcce3d4e2b95d3459fe57c2378124[m
Merge: 5abbdddb 00242432
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Wed Jun 14 16:21:49 2023 +0200

    Merge pull request #155 from vr4vet/more-signboards
    
    More signboards

[33mcommit 00242432baf8073152dd3ca95ae075d138a9996a[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 14 16:09:29 2023 +0200

    fix: adjusted the height of the benches and made the rack smaller #153

[33mcommit bc042fbf878510a8fae49963dc77b4c0f4990448[m
Merge: 902eba6c 5abbdddb
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 14 15:52:59 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into more-signboards

[33mcommit 902eba6cc18835af7bcac45a30b9fc49a952581b[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 14 15:50:00 2023 +0200

    feat: divided tutorial text, added more signs and arrow signs #148

[33mcommit 5abbdddb89740eade7b67aa4554a1145fef8cc2e[m
Merge: b21bc69d a69b75fc
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Wed Jun 14 13:42:54 2023 +0200

    Merge pull request #154 from vr4vet/benches-and-coat-racks
    
    Benches and coat racks

[33mcommit a69b75fce6ef5cda1d034765bc9bfa211cfa33c9[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Wed Jun 14 13:33:41 2023 +0200

    fix: static for baking
    
    Made the benches and coat racks static for baking
    
    Refs: #153

[33mcommit 862e9434d9b7df014d22e2a0ce162056ba6cec92[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Wed Jun 14 13:30:49 2023 +0200

    feat: coat racks
    
    Modelled and textured coat rack, and placed multiple in the office
    prefab.
    
    Refs: #153

[33mcommit 9e6566500bdd380f8bcb56938a9c600d650c3b4f[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Wed Jun 14 12:25:24 2023 +0200

    feat: add benches
    
    3D modelled and textured new bench model, and placed multiple in the
    office building prefab
    
    Refs: #153

[33mcommit d90d7e09ec4dc44d7503a7be772c5d3a0a8f9494[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jun 13 15:59:59 2023 +0200

    feat: imported vr4vet tutorial
    
    Currently not working. Am collaborating with Frederik on identifying the
    cause.
    
    Refs: #151

[33mcommit b21bc69d3ac80fbc8011633f98843e0915da0aa9[m
Merge: 9f967cd3 0ac26030
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Tue Jun 13 15:51:23 2023 +0200

    Merge pull request #152 from vr4vet/pausemenu-change-buttons
    
    Change merd buttons and move them

[33mcommit 0ac26030e923bfe037e1795993414a3056e29639[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Jun 13 15:37:48 2023 +0200

    chore: added hologram and soundManager to FishFeed scene

[33mcommit 5764209c39ad3083300ec56cdd18f2e1bbeee2ed[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Jun 13 14:06:18 2023 +0200

    feat: added pausemenu to fishfeeding scene, still buggy #139

[33mcommit 160bbd77f8cf4f2d24faa0caa71e74e7b12262c0[m
Merge: 1aef3b9a 9f967cd3
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Jun 13 12:38:25 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into pausemenu-change-buttons

[33mcommit 1aef3b9a34e01da6eb3411f7043aaa734a290e85[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Jun 13 12:34:42 2023 +0200

    feat: added new slider-button pairs where the button is before slider
    
    Refs: #144

[33mcommit 9f967cd3a79b5df00177ec7d66c3d891ea78ae7b[m
Merge: 803f3ce6 4b3a3bd6
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Tue Jun 13 11:50:43 2023 +0200

    Merge pull request #149 from vr4vet/sound-manager
    
    Sound manager

[33mcommit 4b3a3bd6410803027799c46cc81ac4d5266c26d6[m
Merge: adebb7bd 803f3ce6
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jun 13 11:45:31 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into sound-manager

[33mcommit 803f3ce6bbe0e97ea03259cd184490864be49eff[m
Merge: ec09b135 2f24d8a5
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Tue Jun 13 11:42:24 2023 +0200

    Merge pull request #150 from vr4vet/improve-tutorial
    
    fix: improve tutorial text

[33mcommit 2f24d8a59716d4c002b186dbf4061cc035b1ba17[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Tue Jun 13 11:18:59 2023 +0200

    fix: improve tutorial text
    
    Made text of tutorial pop ups more concise and clear, which should make
    them easier to read and understand quickly.
    
    Refs: #133

[33mcommit 19684ede32809c4bd1406e3f0b34c44a4df81e3f[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Jun 12 16:32:53 2023 +0200

    feat: changed previous merd buttons to VRIF buttons
    
    Changed button model and are not radio buttons anymore since that was a part of the
    sensitivity issue.
    
    Refs: #144

[33mcommit adebb7bd8ff1ecf729ef0bd43af69abc2fd89bf9[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 12 15:23:39 2023 +0200

    fix: mute ocean sounds in office.
    
    Moved the audio source a little to fix the problem. Not ideal at all,
    but there is no way to mute or lower volume within a specified area as
    far as I know. Lowering the volume based on distance from the source
    like this does not work well when entering a building etc.
    
    Refs: #147

[33mcommit 4df7e063bd4ad6b654c106e1a8b47ebcc883ff2c[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 12 14:43:26 2023 +0200

    feat: import sound manager to scene.
    
    Have experimented with the sound manager. It works, very similarly
    Unity's audio source. Doesn't seem to support multiple instances in the
    scene.
    
    Refs: #147

[33mcommit ec09b135e6978e10dae239aad633f89aeee1152f[m
Merge: 08e0a9b5 04335803
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Mon Jun 12 13:47:14 2023 +0200

    Merge pull request #146 from vr4vet/fish-cage-hologram
    
    Fish cage hologram

[33mcommit 04335803b038d1effdfabb61650c784d69c61644[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 12 13:28:09 2023 +0200

    misc: move hologram to the left side.
    
    Hologram is moved to the left side of the control table. This is closer
    to the left monitor. As a result, the player is not required to rotate
    their head so much to watch the hologram.
    
    Refs: #136

[33mcommit 75741258f7ad7fb22d4eed1ca1d1ea7d5baf1b43[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 12 13:12:14 2023 +0200

    misc: remove incorrect comment.
    
    Refs: #136

[33mcommit 9a731b157cf8c0a096b5208a65a557d99993f0e6[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 12 12:55:04 2023 +0200

    fix: hologram position offset and cleanup
    
    Hologram script now has fields for setting Z and Y offsets to compensate
    for strange off-centred positioning of of the MerdCameras. Also changed
    names for the hologram materials.
    
    Refs: #136

[33mcommit a8ef2437f01c9bf26690c8d0a33f38a4e34b427b[m
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 12 10:32:47 2023 +0200

    fix: NullReferenceException and pull from fish-feed.
    
    Refs: #136

[33mcommit 667e3afc2f878169a5ce2acc727d93986d2cb7e7[m
Merge: 4543b4a4 08e0a9b5
Author: Emil Aron Andresen Mathiesen <emilmathie@yahoo.no>
Date:   Mon Jun 12 09:56:48 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into fish-cage-hologram

[33mcommit 08e0a9b5dbd15f3112e4901c1e56b6d73deaca59[m
Merge: e92fd639 45455c5d
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Mon Jun 12 09:51:07 2023 +0200

    Merge pull request #145 from vr4vet/visualization-selected-merd
    
    Visualization selected merd

[33mcommit 45455c5dd8a67496e0c5b93e3786fcbe723a5773[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Jun 11 16:54:29 2023 +0200

    feat/fix: added immersive view to tutorial and fixed timeout bug #138

[33mcommit 4543b4a44cbb0c4a917bb964c5f713efb2775bb5[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Jun 11 16:20:26 2023 +0200

    fix: hologram camera vertical movement speed.
    
    The hologram's y-size should not be divided by two. This was removed, and
    the hologram camera moves vertically all the way as intended now.
    
    Refs: #136

[33mcommit fd1ac7aaf388a308ebc3e8d70413418fc99c6350[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Jun 11 16:04:59 2023 +0200

    feat: base for hologram, and tweaks for materials. Refs: #131

[33mcommit 0658f37a4edced6916ee6f7d6383c6bf931e699f[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Jun 11 14:29:07 2023 +0200

    feat: added bloom/light when the merd button is selected #137

[33mcommit 49af04c9ce813dff8a51ee7fea4001b0839544ec[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Jun 11 12:31:14 2023 +0200

    feat: 3D model of hologram and logic for camera. Refs: #131 and #136

[33mcommit e92fd6395fa5fa9096e151ce0d2bb60d6ee7018e[m
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Sat Jun 10 12:16:40 2023 +0200

    Update README.md with another solution to LFS issues

[33mcommit 6b5606b0ea89abd30ed46187315c248a5d6dfb6d[m
Merge: b8129e7f 4877c59b
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Jun 8 16:27:34 2023 +0200

    Merge pull request #142 from vr4vet/food-visible
    
    fix: fixed and increased the size of the food particles #132

[33mcommit 4877c59bf80083711770b661797b91153f9cd45e[m
Merge: d4bb32ed b8129e7f
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 8 16:24:56 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into food-visible

[33mcommit b8129e7fa8092ab2976a34ab9a46b3d5d0ec8e11[m
Merge: 1b2ccbf3 e0220a6e
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Thu Jun 8 16:23:26 2023 +0200

    Merge pull request #143 from vr4vet/fish-downwards-swimming
    
    fix: fish spreads evenly when swimming downwards

[33mcommit d4bb32edc948ee076842801196c93d1c2a0c2078[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 8 16:05:42 2023 +0200

    chore: reset position to levels buttons

[33mcommit e0220a6e4f860cc735899e97a6f960329560e122[m
Author: asd <dragvoll@NTNU00667.win.ntnu.no>
Date:   Thu Jun 8 15:22:22 2023 +0200

    fix: fish swim downwards to a randomly chose y position when full. Refs: #130

[33mcommit d51fd790e4e14570cc5075ca51ff8226ceebaa52[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 8 15:51:00 2023 +0200

    fix: fixed and increased the size of the food particles #132

[33mcommit 1b2ccbf3095ab3eb234c2edfab1f3b6bb8245a47[m
Merge: 66ccc3f0 51a7cb8a
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Jun 8 13:43:17 2023 +0200

    Merge pull request #141 from vr4vet/food-visible
    
    Fixes general bugs to the game

[33mcommit 51a7cb8a2a61849be4dd70b51cf12d2e96f6c4f7[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Jun 8 13:33:51 2023 +0200

    fix: fixed radio button issue by changing back to BNG buttons #140

[33mcommit b313feb978ea8b2c7841ef31526b73193e421338[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Jun 7 16:29:34 2023 +0200

    fix: fixed modeLoader and Modes. Created a prefab for Modes

[33mcommit 66ccc3f022354dbcbc69c57a47ba6f4003fef70f[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed May 10 11:01:23 2023 +0200

    misc: Code cleanup

[33mcommit 721b52bccfd8aae869c8d95e9a2a658568123a20[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 18:09:02 2023 +0200

    Update README.md

[33mcommit 4a6cc73eab858c94d61a5f8b3fbf87c7e2662a4a[m
Merge: 6b10f11c 0db745b7
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 17:16:58 2023 +0200

    Merge pull request #104 from vr4vet/documentation
    
    Documentation

[33mcommit 6b10f11c62c074df5bf9dd8e28a80c7e8bca55a4[m
Merge: 73804d20 b67c2e13
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 17:07:44 2023 +0200

    Merge pull request #129 from vr4vet/cleanup-and-doc
    
    feat: added documentation for prefabs and fixed missing prefab error

[33mcommit 73804d20fb904d30917c98220749c711cd276ab6[m
Merge: bccb23ee 2defd288
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 17:04:15 2023 +0200

    Merge pull request #126 from vr4vet/tryms-components-docs
    
    Tryms components docs

[33mcommit 2defd28879684193ef5d1dc2b05588e5d5de0423[m
Merge: f1c20e01 bccb23ee
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 17:04:09 2023 +0200

    Merge branch 'fish-feed' into tryms-components-docs

[33mcommit bccb23ee63e234abea41b4787d06c3d9fa7c4825[m
Merge: 93888b83 daaf5b7c
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 17:03:35 2023 +0200

    Merge pull request #127 from vr4vet/emil-readmes
    
    Emil readmes

[33mcommit 93888b83edf3c315aae62ee9433caca1a655ba69[m
Merge: d4a0ffeb 7e6e32fc
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 17:03:25 2023 +0200

    Merge pull request #128 from vr4vet/cleanup-torjacob
    
    Cleanup torjacob

[33mcommit b67c2e1375c663a501756855627568860049f910[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Apr 27 17:02:49 2023 +0200

    feat: added documentation for prefabs and fixed missing prefab error
    
    Added documentation for MonitorScore and CanvasHUD prefabs. Fixed the
    missing prefab error in the MonitorScore prefab.
    
    Refs: #20

[33mcommit 7e6e32fc49453828a39c36ce23deaf1961f8c5a0[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 16:55:24 2023 +0200

    chore: cleanup and format code

[33mcommit b98681610ece6afe8c47fa7afc0a3efb729f901c[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 27 16:40:47 2023 +0200

    docs: document merdcontrols and docs components

[33mcommit daaf5b7c863ac2f60562f0a8d6c721ea405c9081[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 27 15:56:27 2023 +0200

    fix: yet another typo

[33mcommit 30f91bc5ef8a8a137a8b8a0cdc9483ad48e1ac51[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 27 15:51:49 2023 +0200

    fix: typo

[33mcommit 0fa1169057138a3c4f146373a8a286a7e4cff542[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 27 15:50:42 2023 +0200

    fix: categorise better in FishSystem doc

[33mcommit adc92217f5e3822813b4148b30b63d2402c65591[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 27 15:46:30 2023 +0200

    doc: add documentation for Fish component

[33mcommit f1c20e01f05be31caedc7a9edb73cb076ca0dd06[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 15:41:05 2023 +0200

    misc: add tooltips to radio button

[33mcommit a8421173cc37999e162f8e247cb330b74ba964bc[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 27 15:36:37 2023 +0200

    feat: write documentation for fish and fishsystem

[33mcommit aa27e3d768d6831141b94596181d339be7b1b88e[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 15:28:26 2023 +0200

    misc: add basic readme for the fish feeding subdir

[33mcommit 541eec1878e11419ff3e47a2b0a2766b60c33396[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 15:24:54 2023 +0200

    misc: remove unused scene

[33mcommit 712a7c65d225beb26168da114250a2deb1548ca9[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 15:22:23 2023 +0200

    misc: Add ocean documentation

[33mcommit f2fa838222fdf581ac3441b240367699e884ea74[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 15:13:29 2023 +0200

    misc: Add tooltips for fields visible in the inspector

[33mcommit eb73418fbb359864251ded58687fa31695548fa5[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 15:13:11 2023 +0200

    misc: Remove unused marker interface

[33mcommit b550c93f5c587d3bdd86392f8feb95c9f846ba76[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 15:12:03 2023 +0200

    Add documentation for the merd camera.

[33mcommit d4a0ffeb82bf338dbbb92c05a8695bcac9ef94dd[m
Merge: 0e283e09 06b76061
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Apr 27 14:52:39 2023 +0200

    Merge pull request #125 from vr4vet/cleanup-repo
    
    misc: cleanup folder structure, remove unused assets

[33mcommit 06b760614cc7cfb72b1bf5ba205212eaf113e968[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 27 14:41:59 2023 +0200

    misc: cleanup folder structure, remove unused assets

[33mcommit 0e283e0934d42bd547cdcfe17ade7be6c2e0dc8c[m
Merge: 9e9b17b0 5e8d6d55
Author: TrymPet <trym2001@hotmail.com>
Date:   Mon Apr 24 20:22:40 2023 +0200

    Merge pull request #117 from vr4vet/put-together-demo-scene
    
    Merge demo scene into fish-feed

[33mcommit 5e8d6d553ed3b36cecde2d4b74f0f178f2b3399e[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 20:21:49 2023 +0200

    misc: minor nits, object placement, rebake lighting, colluing

[33mcommit 7778bca3623e3fe2cbe84839b6b92a3163772bef[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Mon Apr 24 19:04:23 2023 +0200

    feat: add ocean sounds

[33mcommit 8a5a4fe67272fc2d4b4596f202ef352df765d512[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 19:16:14 2023 +0200

    fix: hints and minor bugs

[33mcommit cbbf919ed932c6286a7117d74c038b9f7a8e40f6[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 17:39:58 2023 +0200

    fix: null reference exception

[33mcommit 7c7193c52d70a23f49a73b49031ca2ddf1daa194[m
Merge: c71fa3ea 28a8f9d1
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 17:39:30 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit c71fa3ea7ef69fd00719b31b91b3813477fa6096[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 17:39:08 2023 +0200

    misc: update hints, etc.

[33mcommit 28a8f9d1131df5838f89b15fd94fe17b64963ce9[m
Merge: 78400aae 158a3d6d
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Apr 24 17:23:43 2023 +0200

    Merge branch 'put-together-demo-scene' of https://github.com/vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 78400aaed856a2b4c6adb5a1a9e11819c5f31519[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Mon Apr 24 17:23:34 2023 +0200

    feat: added text on monitor for current merd shown

[33mcommit 158a3d6dd60dadcfcaae52683e5e4c01aef2a3ff[m
Merge: 7bda72ad 9e9b17b0
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 16:11:03 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into put-together-demo-scene

[33mcommit 9e9b17b0313c8152a211a5dc92e469cdfcddf1f6[m
Merge: f9866178 db7cb2f6
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Mon Apr 24 16:09:45 2023 +0200

    Merge pull request #123 from vr4vet/improve-tutorial-prefab
    
    Improve tutorial prefab

[33mcommit db7cb2f6d4d0263b6cf458cdbddd3f4f9f814bee[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 15:14:34 2023 +0200

    fix: rounded rect fog transfer

[33mcommit 893728c2ceaf324740b6470509f775e9d02526b4[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 15:06:52 2023 +0200

    choire: remove unused types

[33mcommit 08242c97c81fbcb432f8a6a8cf5f5594a34f302b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 14:55:06 2023 +0200

    feat: add resize support, add gizmo

[33mcommit 99d07704dbc25a0d642c904c771d91b52104c7cf[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 14:53:26 2023 +0200

    fix: tutorial dismissal

[33mcommit be4dc336db24bf10283f7e92397c4e4b9050359c[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Apr 24 14:51:14 2023 +0200

    feat: add reset functionality to tutorial

[33mcommit 7bda72adcc69ed9a11a456ccdb085737dda7d9b8[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 21:36:58 2023 +0200

    apply overrides

[33mcommit 8f5280c994ec5b37abb781e2f37f895fe14d438c[m
Merge: 76f7bec0 edee14b3
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 21:31:03 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 76f7bec07ae8ddf0d88f810d1a1007850e2828e2[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 21:30:06 2023 +0200

    fix: merd buttons

[33mcommit edee14b3ee86a37a516bd48b9e79099065a3d24e[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Apr 23 21:17:57 2023 +0200

    feat: added ChangeCameraHint to tutorial

[33mcommit c2e2f3238402ba24608c8d941b53d04656bb8441[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 20:23:42 2023 +0200

    feat: new hints

[33mcommit 640f8b42b196c95119532e29f232e9bd80a2c1e9[m
Merge: 0f5fcb79 8c5c44d0
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 20:23:22 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 0f5fcb7970f3018e4e229fdd5dd1fd89d3f70a9f[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 20:19:03 2023 +0200

    feat: add new hints

[33mcommit 8c5c44d0fc0feb05161dec5c2c2ff662512f5375[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Apr 23 19:40:34 2023 +0200

    misc: finetune position of objects on tables
    
    Refs: #114

[33mcommit a48770412a089acb2b0880a8fcfedfd5bb1ab1c9[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 19:35:53 2023 +0200

    feat: update location of current level

[33mcommit 8ef9b96382d9d88386acd6e36c334cbd80ccc632[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 19:03:05 2023 +0200

    feat: new buttons part 2

[33mcommit 32cb56ad8333ca57c9ef067430ea1211f7afb11c[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 19:00:44 2023 +0200

    feat: new buttons part 1

[33mcommit 011b80f75d7f1d8b24541791206b3c62f947a192[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 18:22:51 2023 +0200

    feat: add new buttons

[33mcommit fcf42fe60dda52a755e96beb75daf89156af4962[m
Merge: d75ec817 f9866178
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 18:22:31 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into put-together-demo-scene

[33mcommit f9866178b8e0499d326a53f00eb7893c51bcab79[m
Merge: f697f96b 79eb358d
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Sun Apr 23 17:52:58 2023 +0200

    Merge pull request #119 from vr4vet/new-merd-buttons
    
    New merd buttons

[33mcommit 79eb358df0274463ff49867a9245f1a91bb87c2e[m
Merge: 655633cf f697f96b
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 17:50:42 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into new-merd-buttons

[33mcommit d75ec817ed7f9ac0fcd188a3b97b37fc93436f53[m
Merge: e44ecb27 bd0595a3
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 17:48:54 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit e44ecb2750229df5d7ae3285f79586b634839304[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 17:48:35 2023 +0200

    feat: add new control panel in demo scene

[33mcommit aee33d0876aae1fb2942e104b105725875b6ffe3[m
Merge: fe69e509 f697f96b
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 17:47:06 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into put-together-demo-scene

[33mcommit f697f96b10392dd4836a1f42438e042c74b0b88e[m
Merge: 38d870a4 99dfe527
Author: TrymPet <trym2001@hotmail.com>
Date:   Sun Apr 23 17:09:40 2023 +0200

    Merge pull request #107 from vr4vet/control-surfaces
    
    Cohesive Control Panel

[33mcommit 99dfe52713467934a46527605cd1fd2e8a6564b1[m
Merge: 0636d06b 38d870a4
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Sun Apr 23 17:05:03 2023 +0200

    Merge remote-tracking branch 'origin/refs/heads/fish-feed' into control-surfaces

[33mcommit 0636d06b71f3847467ddcca19201e2be39804582[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Sun Apr 23 16:44:56 2023 +0200

    feat: make new controlpanel

[33mcommit bd0595a3c9a6201c988727696e5e40218c4468d5[m
Merge: fe69e509 da2597e4
Author: TrymPet <trym2001@hotmail.com>
Date:   Sun Apr 23 16:36:59 2023 +0200

    Merge pull request #122 from vr4vet/improve-wastage-bar
    
    Foodwastebar has different colors based on slider value

[33mcommit fe69e509a532e68367c91586bc6b148b22caf8f3[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:34:47 2023 +0200

    perf: use medium quality preset; set AA to 4xMSAA

[33mcommit 7a88e19bbb99f4a2e9d57b0550f25f489320f604[m
Merge: cbf7d11d 7688d2e4
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:24:15 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit cbf7d11d21352f4b19b5bdb0537b97521085587b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:24:06 2023 +0200

    fix: ocean performance

[33mcommit 977abdd2087d3c5ad983ca674dd6121d9f8ee7bf[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:18:02 2023 +0200

    fix: dock lightmap

[33mcommit 1a2f0499e66d06b5b4301d9ccf9877672ca64794[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:17:40 2023 +0200

    fix: update one of the hints

[33mcommit 6d21040daff110bb36a65ccfcdf1ba9ac8dc6018[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:17:27 2023 +0200

    feat: remove some stuff (?)

[33mcommit 00892e115fcd74c9d6757feae95560be90e2f77c[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:15:51 2023 +0200

    fix: start game outside of box collider

[33mcommit 6a39d9463599e3a612136329552fb4ccd2ce63f6[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Sun Apr 23 16:00:44 2023 +0200

    fix: update fish LOD

[33mcommit da2597e4bd28e60bff5e8bc36c6edc074c997f24[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Sun Apr 23 15:18:44 2023 +0200

    feat: foodwastebar is a different color based on the slider value

[33mcommit 7688d2e44d451c07334eed62d74e8c3f12283e97[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Apr 23 13:50:58 2023 +0200

    fix: make collision boxes surrounding dock taller
    
    teleporting onto the surrounding collision box should now require more
    effort. The player must deliberatly point the teleport high up in the
    air if they wish to go out of bounds.
    
    Refs: #114

[33mcommit 5afac92ade7e306c166347fff104d16e2aa77f95[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Apr 23 13:40:06 2023 +0200

    fix: clipping through stairs
    
    it was possible to jump through the stair case from underneath. This is
    fixed by extending an invisible wall from the stairs to the floor.
    Unfortunately, this means the player can't walk up to the window under
    the stair case, but I think this is fine.
    
    Refs: #114

[33mcommit 03c580c7c7ebb4074e932e02248e9e2d7d97833f[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 22:03:58 2023 +0200

    feat: reduce sun intensity, rebake light

[33mcommit 12f3983c24d49eac39b68f5f10d519c73f244915[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 21:50:50 2023 +0200

    fix: various changes to demo scene in preparation for demo

[33mcommit 215fbfc15cd0115535cdc3e7c9a53051b516922b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 21:49:59 2023 +0200

    feat: new baked light maps and occusion

[33mcommit a8c6fd2f1c0d2c772246f3ce25845d283ce1b84b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 21:49:42 2023 +0200

    fix: levels prefab with new buttons

[33mcommit 6357bcebea0c504941caacc1924f442b56b084e7[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 21:49:16 2023 +0200

    fix: control panel scale

[33mcommit 8658967658c2bfad0460d8be06b148f21c1a29a7[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 21:48:37 2023 +0200

    fix: realism time (5m), no tutorial in realism

[33mcommit 3cbd1d0170cfa74c77637b3ab67c8b440360c642[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 21:41:18 2023 +0200

    fix: tutorial dismissal, HUD, score

[33mcommit da89bd7f050f0c78201a4d36b89719f9521aa3dd[m
Merge: d6313c7b 38d870a4
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 19:56:25 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into put-together-demo-scene

[33mcommit 38d870a4bb0a077342b9da3469f9f03e338218e0[m
Merge: b956511d c782ddd1
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 21 19:43:19 2023 +0200

    Merge pull request #108 from vr4vet/mode-imporvements
    
    Integrate Gamemode

[33mcommit c782ddd1d8ec93579d3e8055c81653f9233c3ec2[m
Merge: 3574120b b956511d
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 21 19:43:00 2023 +0200

    Merge branch 'fish-feed' into mode-imporvements

[33mcommit d6313c7b2ca3fdd6796512595684b6734f390db1[m
Merge: 8c405171 8c060b29
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 19:36:59 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 8c40517172167025c83ba971894064b71653ad8e[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 19:36:16 2023 +0200

    fix: slider static, furniture on desk, desk height, start game box collider

[33mcommit 8c060b29448918c6901b2989146cfea8b62a03fb[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Apr 21 19:34:51 2023 +0200

    fix: stairs from office to meeting room
    
    changed the slope position/scale/rotation to make it possible to leave
    the office
    
    Refs: #114

[33mcommit 3574120b312a1134edd03d116496bed5fd390c48[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 21 18:12:46 2023 +0200

    refactor: move to btn pr level

[33mcommit e378052a0079a0bb4080b12f3d45344966cd4bd4[m
Merge: 30e728d6 c117cd40
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 17:56:01 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 30e728d66410ff353ba455ba24fb69fb82b5b7fc[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 17:55:49 2023 +0200

    fix: update UV lightmap for the office to fix unlit parts

[33mcommit 0bfd021cff90ff457e73f0651618ad2c66a87f96[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 21 16:18:28 2023 +0200

    fix: resolve bugs related to merge

[33mcommit c117cd405bad90849a3eb4772884b87a279e4121[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Apr 21 15:54:21 2023 +0200

    fix: shrink desk collision box and move chair a little
    
    this should prevent the player from getting pushed back when interacting
    with the controls
    
    Refs: #114

[33mcommit 8fe65f3e66c158d261e9d790cb9b65558665e7b8[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 15:18:44 2023 +0200

    feat: use better light map compression to fix compression artifacts

[33mcommit 3bd2a2769afff1a5a5d23d82cfdd51cd4a794ff0[m
Merge: 42b11a66 cf57ed50
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 14:50:03 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 42b11a665576d43cd3d166fb99ef95c7c273c36b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 14:48:42 2023 +0200

    feat: new baked lighting

[33mcommit cf57ed508a8584a089bfb81aa8046ff18f6aa01f[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 21 14:28:09 2023 +0200

    fix: increased ocean

[33mcommit 99dbbe58c2d467218764e6798bce0226f09b4030[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 21 14:11:57 2023 +0200

    fix: decreased size of the UnderwaterOcean and shadows

[33mcommit 2bab581525f48efdcb0c538700a0761b66bb8bcf[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 21 13:49:13 2023 +0200

    fix: fixed office stair collider after scaling

[33mcommit f0e9908008eba53299e5a2267d9bf9fad636cc22[m
Merge: 58d1ae53 df6be7af
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 13:37:50 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 58d1ae53fe341a93f775a7999c94b71166f8a0a7[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 13:34:45 2023 +0200

    feat: add new baked lighting

[33mcommit df6be7af6c60e26abaf75342fc3bace703f1eb21[m
Merge: 4b8c23fe 3b0926e2
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 21 13:19:43 2023 +0200

    Merge branch 'put-together-demo-scene' of https://github.com/vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 4b8c23fec5c995bd08352df9755a08604b213215[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 21 13:15:40 2023 +0200

    feat: moved and positioned things better with the scaled house
    
    Positioned monitors closer to the controls. Changed the language of the
    text to the startGame tooltip.
    Changed also some settings in Fish.prefab to improve LOD.
    
    Refs: #114

[33mcommit eca62cb30b133ac99eb60b930073738f98ef0bb5[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 21 12:25:57 2023 +0200

    feat: increased scaling of house and added fog to demo scene
    
    Refs: #114

[33mcommit 3b0926e24315a2716da33a8e3e7a03064fd4f1a9[m
Merge: 696da1a0 40b18c44
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 12:20:42 2023 +0200

    Merge branch 'put-together-demo-scene' of github.com:vr4vet/Blue-Sector into put-together-demo-scene

[33mcommit 696da1a0ed983c3bc9d3c4909124bb8009fd60c6[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 12:17:55 2023 +0200

    feat: make new office static

[33mcommit 076f219af4b50a2cd29a3845ca91794d2455c4d1[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 21 12:02:38 2023 +0200

    feat/fix/perf: finalize mode implementation

[33mcommit 40b18c446623c6d46b6e0d7973daf4a9f4c17804[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Apr 21 11:57:45 2023 +0200

    feat: make desk taller
    
    the table's legs are now longer to make use of the controls more
    comfortable
    
    Refs: #114

[33mcommit 5076f1bb211f807b7101df7e25602bcd7962a7f9[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 11:16:42 2023 +0200

    feat: fix camera track

[33mcommit be727c9ab54d5f349c088fe363d2c4325c3bb212[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 10:31:31 2023 +0200

    feat: remove block

[33mcommit b956511de833a7f9a5205c327f2a782bab064735[m
Merge: bd38af5f 6af59073
Author: TrymPet <trym2001@hotmail.com>
Date:   Fri Apr 21 11:18:53 2023 +0200

    Merge pull request #115 from vr4vet/remove-block
    
    feat: remove block

[33mcommit 6af59073a07e43e94affe3c4e10c1550617bea07[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 11:16:42 2023 +0200

    feat: fix camera track

[33mcommit 7cfce76a3c9bc8de15eb0ad8c6c366f242ae840b[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Apr 21 10:50:22 2023 +0200

    feat: replace old building with building with fixed stairs
    
    the OfficeComplete prefab now has slopes that allows the XR Rig
    Rigidbody to walk up steps and stairs.
    
    Refs: #114

[33mcommit 2cc14727179f6ac80d609659daf4caad55f93a92[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 21 10:31:31 2023 +0200

    feat: remove block

[33mcommit bd38af5f17649c756642f64194e50eae4cef424a[m
Merge: 8369e17f bc093703
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Fri Apr 21 00:29:55 2023 +0200

    Merge pull request #113 from vr4vet/revive-dead-fish
    
    Revive dead fish

[33mcommit bc09370360e7d65c17dd4f8241d843898144d027[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 21 00:22:26 2023 +0200

    feat: add fog to LOD scene

[33mcommit fa694126ac1ef853c5efa3f7ee607370fbb0cfbc[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Apr 20 23:47:48 2023 +0200

    feat: revive dead fish when the merd becomes idle after game
    
    Co-author: Emil Aron
    Refs: #85

[33mcommit 8369e17fb94bb31a6cf2d3802280b715dbc54abc[m
Merge: d13f81fd 1189308c
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Apr 20 22:31:36 2023 +0200

    Merge pull request #112 from vr4vet/fix-score
    
    Fix score

[33mcommit 1189308c5235b2a71482d2c5a851b4eb5c6b16bd[m
Merge: ac88f62f d13f81fd
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Apr 20 22:08:04 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into fix-score

[33mcommit d13f81fd45d6f0c148b150d610bded6b383dcdb0[m
Merge: 3f5ae6b4 bd0584cb
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Thu Apr 20 22:06:25 2023 +0200

    Merge pull request #111 from vr4vet/fix-lod
    
    Fix lod

[33mcommit bd0584cb6c9932343ea63ef40ae090685c4abb73[m
Merge: 7c7306b2 3f5ae6b4
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Apr 20 22:00:24 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into fix-lod

[33mcommit 7c7306b27a3567fc2da66f6cb3afdaa9fc741b47[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Apr 20 21:54:13 2023 +0200

    feat: adjust LOD distance
    
    Adjusted the distances for when the fish becomes a sprite. More fish
    should now be rendered as a 3d model.
    
    Co-author: Emil Aron
    Refs: #100

[33mcommit 3f5ae6b45ae3ade8495f63f3aaa4f11de2add837[m
Merge: 9a08168e 8b9597b7
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Apr 20 21:08:43 2023 +0200

    Merge pull request #110 from vr4vet/fix-stairs-and-other-obstacles
    
    Fix stairs and other obstacles

[33mcommit ac88f62fe1f7ab3d0d8bb30ee2ec905d180f4528[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Apr 20 20:40:07 2023 +0200

    feat: change scoring so it doesn't decrease
    
    Scoring won't decrease in the score and it won't be negative. Modified
    scoring so it is also not dependent on the amount of fish in the merd.
    Have also increased the time intervals the merds use between Full, Hungry and Dying stages. This still needs improvement since the slider doesn't work in the demo scene.
    
    Refs: #98

[33mcommit 4d28f290b09d3ca07264670345e8f23e7e7d2a49[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 20:18:27 2023 +0200

    misc: further changes that should have been commited
    
    Refs: #100

[33mcommit 655633cf729993902174845baab9b50c03e407d9[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 20 19:38:14 2023 +0200

    feat: new merd buttons

[33mcommit aed79e5688b5bc75b58f731217788dbee8b4d15a[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 20 19:36:38 2023 +0200

    feat: new merd buttons

[33mcommit b23bc09a78c329b9013eaf7d6b0ecd6f5105d158[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 19:23:07 2023 +0200

    feat: made LOD visible
    
    Adjusted LOD-bias in project quality settings, and played around with
    the colors and LOD distances until I found something that is better.
    Still not perfect, sprites does not seem to work that well with the
    shader, but will experiment more on more powerful hardware.
    
    Refs: #100

[33mcommit 8b9597b7936c3714d5de41767b2b2c6e5d141527[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 15:57:13 2023 +0200

    feat: save the updated scene
    
    the scene which this issue is solved in is called
    "FishFeedingStairsFixed"
    
    Refs: #95

[33mcommit 08a0ef158110ffa0caae80c445e3b74c376da306[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 15:53:24 2023 +0200

    feat: make a prefab with fixed stairs
    
    there are now prefabs for the stair with the custom colliders, and for
    the house with the slopes and custom stairs included. This is called
    "Building2_with_custom_stairs".
    
    Refs: #95

[33mcommit 1bdd62358c6daecbf3da185a045d04ac678075d7[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 15:50:12 2023 +0200

    misc: add forgotten painting on wall
    
    we have forgotten to put up the painting model that I made, so that is
    now hanging on the wall next to the office
    
    Refs: #95

[33mcommit 2396cd453955f64dbd21a3247f506743d8fa3638[m
Merge: 674036a0 9a08168e
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 15:38:39 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into fix-stairs-and-other-obstacles

[33mcommit 674036a0144f675e9fa3a5fe3b3ffb8daba9a9f1[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 15:19:31 2023 +0200

    feat: add models for colliders
    
    new custom model used as a mesh collider for the stairs. there are also
    planes used as slopes in front of the front door and steps joining the
    meeting room and the office
    
    Refs: #95

[33mcommit 15687e19d66144703915c8979642865070e6100e[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Thu Apr 20 14:08:44 2023 +0200

    feat: separate house and stairs model
    
    this allows for a custom mesh collider for the stairs, while keeping the
    standard mesh collider for the house
    
    Refs: #95

[33mcommit 9a08168e294c3cbdc5495f47754f46395ee0363c[m
Merge: 901c74f3 1b7e5393
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Thu Apr 20 13:15:13 2023 +0200

    Merge pull request #106 from vr4vet/performance-improvements
    
    Performance improvements

[33mcommit 1b7e5393f4b70112539b9198f51be665ac642d6a[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 19:57:02 2023 +0200

    feat: add baked occlusion culling

[33mcommit 3345c4fabe89b0fbf658359a549957980fdd4614[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 19:56:05 2023 +0200

    feat: improve fish update-loop perf.

[33mcommit d7d83524b813921ec695d33ced95eff2ee284ead[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 17:19:04 2023 +0200

    feat: enable GPU instancing for fish

[33mcommit f1fc9a2ec9b9322371e46428eb1bb38f4aee56d6[m
Merge: beab2c01 dfed1936
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 16:40:42 2023 +0200

    Merge branch 'fix-table-inside-house' into add-activation-zone

[33mcommit dfed1936ae1a167676274a9578a0314f97ed415a[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 16:37:36 2023 +0200

    fix: make slider renderer static

[33mcommit 6cf5eb0cf6fce3315a6adeea7e0971a1af08511e[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 16:36:58 2023 +0200

    fix: control panel missing mesh

[33mcommit 22f1ac59be161558015377ca4782ba663a76b968[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 16:30:33 2023 +0200

    fix: put player outside

[33mcommit 722bf9d43c39102cfa37173d194efc6bc060155e[m
Merge: 94479668 901c74f3
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 16:27:52 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into fix-table-inside-house

[33mcommit 0db745b7fe9d672ebbfeb940f6edeb4b0e7ab74a[m
Merge: 036f6b75 b1f0679f
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 19 16:26:07 2023 +0200

    Merge branch 'develop' into documentation

[33mcommit 036f6b758237d0fbb44a8b050cc924e08d867671[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 19 00:15:11 2023 +0200

    docs: add lfs-fix description
    
    added description on how to fix common lfs error.

[33mcommit 52e5980e5203e2de85a88bc75ecdabbc0f9d2b94[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Sun Apr 16 11:32:02 2023 +0200

    chore: update gitignore

[33mcommit fd13d587f4869754ef480382146b2b62b418f03d[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Sat Apr 15 13:53:50 2023 +0200

    docs: move code of conduct link

[33mcommit 21f07dfcec86cf2aaf09e84309c766f4c8571a09[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 10:13:33 2023 +0200

    chore: formatting

[33mcommit 70d9b353e6906856ed81584ee6b1716dda9efd8d[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 10:02:42 2023 +0200

    docs: update readme and add contributing

[33mcommit 901c74f32bac2b6f2d3ad8df9444fb3379ef7858[m
Merge: d81c85d4 d733687f
Author: TrymPet <trym2001@hotmail.com>
Date:   Wed Apr 19 16:22:02 2023 +0200

    Merge pull request #102 from vr4vet/bs2-main
    
    Merge changes from Blue-Sector2

[33mcommit beab2c016308b835ce141841f3294558bed3b791[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 16:20:30 2023 +0200

    feat: add trigger for 'Game.inActivatedArea'

[33mcommit d733687f8620696c43e5114772b64f49ffba4a9c[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Apr 19 16:10:52 2023 +0200

    fix: fix bug from levels in Game script
    
    Fixed bug from levels/modes in the Game script. Added the call function
    to the tutorial object so the function gets called when tutorial is
    completed.
    Slider in the FishFeedingMoreComplete scene might wrong but since we are
    creating a new control panel I left it there for now.
    
    Refs: #94, #53

[33mcommit 944796681f8109578c4acc938b10673467fd567c[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 15:47:36 2023 +0200

    apply lighting settings

[33mcommit 197164c916250407aa1ac2112d21c94b8f57569f[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 19 15:27:00 2023 +0200

    perf: reduce unnecessary checks
    
    reduced amount of unnecessary if checks in Game.cs by returning early in
    update function if game is started.
    
    also removed unused checks for button input, as changing of modes has
    been moved to ModeBtnBridge.
    
    Refs: #66

[33mcommit 226df539670b1c0d02df7bca6eaaf6c429283627[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 15:16:20 2023 +0200

    fix: global sun is baked

[33mcommit 3e76c64301400e600232199122b528ce4863b07b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 15:12:18 2023 +0200

    feat: add more lights

[33mcommit 8277174e8b0a8f41cf2cffb7b95e6af16ccfd0f5[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 15:10:54 2023 +0200

    feat: make light source part of lamp

[33mcommit 6fe93848d8f156ecb9b2899737fac3d8ed126d38[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 15:04:58 2023 +0200

    fix: remove duplicate table

[33mcommit d9d8eefda9c24dd7acf334b03f6884406114faad[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 14:08:03 2023 +0200

    bugfix: put game start tooltip inside house

[33mcommit d37e7021166a369418d921dd6284a8e202e7cd26[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 13:48:43 2023 +0200

    bugfix: Make the table work inside the house

[33mcommit feff6316fad711185072b5c0e580704b2643a937[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 12:56:41 2023 +0200

    hotfix: manually port remaining changes and missing references

[33mcommit b5d8b271e1fb40ccdb2fc1b370d867d1c3444f07[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 12:28:01 2023 +0200

    nit: delete files.txt

[33mcommit fb985e511248a2f7e9eeb023f7ac7e7acabb7ddd[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 12:23:01 2023 +0200

    hotfix: import changes in Unity

[33mcommit 6025d78476d67b39802d53f5d8af9827acf776c0[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 12:21:08 2023 +0200

    hotfix: delete deleted files

[33mcommit 4b7884b2080128cfc2fe9c43f282b3b8160d858f[m
Merge: d81c85d4 85402df7
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 12:12:54 2023 +0200

    Merge branch 'bs2-main' of github.com:vr4vet/Blue-Sector into bs2-main

[33mcommit 85402df79b9cbde5ff8b8402852606bb888e32d5[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 19 11:42:47 2023 +0200

    hotfix: revert changes to .gitattributes

[33mcommit bcc2265324ab9d88cb11472cb0cb02f7e36d3124[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 14 11:28:54 2023 +0200

    Merge changes from blue-sector2/master (demo 1)

[33mcommit bd52f99ef356e963b49828a6a0bc72445e19761e[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 19 01:37:34 2023 +0200

    feat: add mode button prefabs
    
    Refs: #66

[33mcommit 7d1e4788577e5d480855849ecabcb9034dac3e86[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 19 01:29:44 2023 +0200

    feat: add bridge script for mode button
    
    Refs: #66

[33mcommit 50f7beff3c064ae779ce1f234824d28b2d7f8e18[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Tue Apr 18 23:41:54 2023 +0200

    chore: update scene
    
    update scene for testing purposes.
    expected behavior still functions.

[33mcommit d81c85d4cf70e6e74dcb74411bed7ec07cb8e3f9[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Tue Apr 18 23:27:30 2023 +0200

    fix: spelling
    
    refs: #91

[33mcommit 8f4a1b5e1e2ee420d21e05f9556480b830d22229[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 14:18:13 2023 +0200

    rebake lighting

[33mcommit aadf6bf4d7fa46668a2ec7f9a5a9ac1d285de656[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 13:48:08 2023 +0200

    disable desk

[33mcommit 81346c347576d87983d3280fbca1917b7c781eb1[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 13:43:27 2023 +0200

    fix missing knob

[33mcommit 92b8328711ee1fc798b060b14bb2201fbbc6396f[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 13:33:10 2023 +0200

    baked lighting

[33mcommit e691d59a07b6f2cbb4f67bd3d4636339a9b9d638[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 13:22:14 2023 +0200

    apply overrides merd camera joystick; fix ocean

[33mcommit 58e1939f200410489705058a501379c9ac7cb76c[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 13:17:36 2023 +0200

    fix null refernece exception

[33mcommit a727e5fcef5b19a8d4bf3c26b4c580d1f2e80251[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 13:17:17 2023 +0200

    fix null refernece exception

[33mcommit be0fda292a52d9d8fcaca9333beb6fb65933b7e4[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 13:08:49 2023 +0200

    fix tutorial

[33mcommit 3ef8f8ace869bd5c57c2b6b370323a6bfb448695[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 12:34:43 2023 +0200

    hook up control panel

[33mcommit 682f8e24333ceef494324e21c797ed129e4e6346[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 12:34:31 2023 +0200

    fix typo

[33mcommit 96477040a0f0810400f2704ccb99f39165cc4070[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 12:34:18 2023 +0200

    place down merd

[33mcommit 4754cc80c75d562093342eeed163d27791f3da3c[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 12:34:02 2023 +0200

    don't use LFS

[33mcommit 024f0dd06210c7df0bda9dea5db1c64ae0c8199a[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 11:51:22 2023 +0200

    hotfix: SlideBridge.cs

[33mcommit e95113b5d7bd77e970bec403ed1c97dce42b0dad[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 11:48:56 2023 +0200

    hotfix: SlideBridge.cs

[33mcommit f428ea7432b4092968c6a7c5403262db1d690480[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 11:03:11 2023 +0200

    hotfix: SlideBridge.cs
    
    update case

[33mcommit a13f27073212b512c86d2ec8aea7f35851eb1441[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 10:55:34 2023 +0200

    hotfix: FishSystemScript.cs
    
    change needed bars to public.

[33mcommit 4c9394daeaf6d66383c016cfe8d4bac55e3544ef[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Apr 14 12:16:58 2023 +0200

    tmp

[33mcommit 171750a1c79acc8af11e6597fe1463775be6ecab[m
Merge: ba4213a0 0f4f3505
Author: TrymPet <trym2001@hotmail.com>
Date:   Fri Apr 14 12:16:08 2023 +0200

    Merge pull request #91 from vr4vet/torjacob-patch-1
    
    hotfix: FishSystemScript.cs

[33mcommit 9886b5f5188db151096eeea18a8f341ba6a671ca[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 22:39:22 2023 +0200

    feat: make so modes can disable tutorial
    
    Made so tutorials in scene are disabled if mode has attribute signifying no tutorials.

[33mcommit 4b6aac7911c4c568ce5b764cd46417b50f62e756[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 21:22:55 2023 +0200

    wip: improve mode change effect on game

[33mcommit 4c269a6bb500637f794ff58dda07ecf1ced557ed[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 17:07:27 2023 +0200

    fix: merge conflicts

[33mcommit 71a4ca26f9e4be3355a5383e102c8cb270edce30[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 16:41:03 2023 +0200

    feat: control panel
    
    added control panel, making sliders collected.

[33mcommit 58e0398546d5d950df235631d8cecf6cd9193688[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 12:58:47 2023 +0200

    fix: FFsliderprefab funcitons as expected

[33mcommit d219884bd12394150a7043e081026d0a317ae593[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 10:31:12 2023 +0200

    feat: add ffslider prefab
    
    Add prefab for fish-feeding slider

[33mcommit 817bf43fc06fd49a780dcbb865acd02fc9c1a777[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 12 10:43:58 2023 +0200

    fix: merdcontrols funciton as expected

[33mcommit 0f4f3505559a56e6777c74461ec6d89537bb3b40[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 11:51:22 2023 +0200

    hotfix: SlideBridge.cs

[33mcommit 8d8cb9afd85664ef03b59d2a9b0999417785ada6[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 12 16:22:55 2023 +0200

    feat: update levelloading
    
    modes are now loaded instead of levels.
    possible improvements that could still be made include updating scoring algorithm and threshold for failure / hunger depletion rate based on values in a level.
    However, they are by definition implemented  - and for now change time

[33mcommit 73d72d5123819e2d85b6ba68e9110cfa85fdba32[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 11:48:56 2023 +0200

    hotfix: SlideBridge.cs

[33mcommit 38708d6375c025dd53999913db13df83d7de5d63[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 22:53:23 2023 +0200

    feat: make so modes change difficulty
    
    Note: some tweaking on modifier value for each mode may be necessary, however - the concept now works.

[33mcommit 3bc3ca1ee1292092773d96094454ac7db80faf77[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 22:39:22 2023 +0200

    feat: make so modes can disable tutorial
    
    Made so tutorials in scene are disabled if mode has attribute signifying no tutorials.

[33mcommit b0fe193ecdfc6b97883b3f26de742f0d6c6a4412[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 21:22:55 2023 +0200

    cherry pick 4094cb569e739852ea5716d402af761e8fdc3d55

[33mcommit 856564ad48869ad922a4ae3c7344f2078de3cba5[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 14 11:28:54 2023 +0200

    Initial commit

[33mcommit 35529ca1d1ce38386dcce3f05394258d366595a9[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 14 11:07:42 2023 +0200

    first commit

[33mcommit 76a9901e23d21a9639d0538fab9a101af1c01841[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 11:03:11 2023 +0200

    hotfix: SlideBridge.cs
    
    update case

[33mcommit 03b3a4d93371688ac61bae8bda0e0c2ac2c4cb69[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Apr 14 10:55:34 2023 +0200

    hotfix: FishSystemScript.cs
    
    change needed bars to public.

[33mcommit ba4213a0680c147fc4030231f62b37144b5f8638[m
Merge: 48d4b0da b5906cf8
Author: TrymPet <trym2001@hotmail.com>
Date:   Fri Apr 14 10:23:40 2023 +0200

    Merge pull request #89 from vr4vet/control-surfaces
    
    Control surfaces

[33mcommit 48d4b0da51b94b8721bb80de195be62690b22bf3[m
Merge: f88a3f43 8c1c688d
Author: TrymPet <trym2001@hotmail.com>
Date:   Fri Apr 14 10:23:30 2023 +0200

    Merge pull request #90 from vr4vet/mode-imporvements
    
    Mode improvements

[33mcommit 8c1c688dbfa6ddc6f405f2b128defcf0e22cb76a[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 22:53:23 2023 +0200

    feat: make so modes change difficulty
    
    Note: some tweaking on modifier value for each mode may be necessary, however - the concept now works.

[33mcommit bbcacc6a00d675630b446f0d439341aeaa9fe669[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 22:39:22 2023 +0200

    feat: make so modes can disable tutorial
    
    Made so tutorials in scene are disabled if mode has attribute signifying no tutorials.

[33mcommit 4094cb569e739852ea5716d402af761e8fdc3d55[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 21:22:55 2023 +0200

    wip: improve mode change effect on game

[33mcommit b5906cf81eebdb280d45d8b3928ef615cb7468b8[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 17:07:27 2023 +0200

    fix: merge conflicts

[33mcommit c8ee09457203af39e37248c519af80fdfd14fdae[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 16:41:03 2023 +0200

    feat: control panel
    
    added control panel, making sliders collected.

[33mcommit f88a3f43f94a309bc1431b7b89e113617f9a1476[m
Merge: 1481f70c 5f46ad09
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Apr 13 16:21:04 2023 +0200

    Merge pull request #88 from vr4vet/level-load-refactor
    
    feat: update levelloading

[33mcommit 51c4d0b20ff9a48a5c4e318340a3f726cfc41e3f[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 12:58:47 2023 +0200

    fix: FFsliderprefab funcitons as expected

[33mcommit 1481f70c9d3d525113fbb3815de37a13fa5a2e13[m
Merge: 0a551240 bfb87343
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Apr 13 12:27:26 2023 +0200

    Merge pull request #87 from vr4vet/finalize-scene
    
    Finalize scene

[33mcommit bfb873436d5627807be36fc3c92287e80924d0e6[m
Merge: 85af2245 0a551240
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 13 12:09:53 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into finalize-scene

[33mcommit 85af2245cf353c7de22c64fdaf2841eb72fb54a4[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 13 12:05:07 2023 +0200

    choire: add new baked lighting assets

[33mcommit 4b950b2727b5e86065534ecddd8c8dc0210f2753[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Apr 13 12:04:01 2023 +0200

    feat: improve baked lighting

[33mcommit 46a27090713581b2125455246fa19ed3d5b8db5b[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Thu Apr 13 10:31:12 2023 +0200

    feat: add ffslider prefab
    
    Add prefab for fish-feeding slider

[33mcommit 0a55124068d6762bd536288d46413c93219757dc[m
Merge: e15f8d82 f5c819c0
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Thu Apr 13 09:13:10 2023 +0200

    Merge pull request #86 from vr4vet/improve-start-game
    
    Improve start game

[33mcommit f5c819c03003a6e2d3606373f98722b208c97ca2[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Apr 13 08:12:49 2023 +0200

    chore: remove unecessary comments in Scoring script after refactoring
    
    Refs: #70

[33mcommit f67d521a3cfd124480d9d5945af0123eab5c9faf[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Apr 12 22:49:03 2023 +0200

    feat: infobox for starting game doesn't show before tutorial
    
    Refs: #53

[33mcommit 0bc7a628f42efdfc1fd15f2c83e15ca26fe0b87f[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 12 17:56:49 2023 +0200

    feat: enable baked lighting for office; weak translucency of ocean

[33mcommit fd8284352c397c0a0e41dc31043809c24d863e00[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 12 16:44:18 2023 +0200

    feat: begin finalizing fish feeding scene

[33mcommit 5f46ad096dc78a009f2123585c0ff854dbd923c1[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 12 16:22:55 2023 +0200

    feat: update levelloading
    
    modes are now loaded instead of levels.
    possible improvements that could still be made include updating scoring algorithm and threshold for failure / hunger depletion rate based on values in a level.
    However, they are by definition implemented  - and for now change time

[33mcommit 210a10be03ddbea869f50f91ce966b0fbfa64e8d[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Apr 12 15:19:57 2023 +0200

    feat: make objects in office grabbable
    
    Made the mugs and books in the office grabbable.
    
    Refs: #69

[33mcommit 0dea8cffcf1db3a432984cf4166fb1756cef37c6[m
Merge: ed98e0f4 e15f8d82
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Apr 12 12:48:14 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into improve-start-game

[33mcommit ed98e0f40a0c54608b4fc171158dd2b93f6a165a[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Wed Apr 12 12:35:50 2023 +0200

    fix: add fish system back to Placeholdermerd

[33mcommit e15f8d82224282623412b91c387b8dc9dbb5576d[m
Merge: 1af1b5cd 52a97bfc
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Wed Apr 12 12:27:04 2023 +0200

    Merge pull request #84 from vr4vet/remodel-house
    
    Remodel house

[33mcommit 52a97bfc5e4955d093a12c89b3afd35ebfbf0d96[m
Merge: 3e709cd9 1af1b5cd
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Apr 12 12:25:19 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into remodel-house

[33mcommit 1af1b5cd64589fff88507a57a1aac29896ae8edc[m
Merge: ddf82342 68d62448
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Wed Apr 12 12:23:15 2023 +0200

    Merge pull request #83 from vr4vet/improve-lighting
    
    feat: baked lighting

[33mcommit 3e709cd9ac104d21778d45b43afc9f39a65e1f32[m
Merge: 2d817eb6 ddf82342
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Apr 12 12:16:04 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into remodel-house

[33mcommit 2d817eb6048190dd1df91c2700039888bf3bd6b1[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Apr 12 12:13:18 2023 +0200

    fix: removed terrain files to prepare for pull
    
    Refs: #63

[33mcommit dd801ed52dea4504da881419cbdcefd3667c301f[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Apr 12 12:08:26 2023 +0200

    fix: add new terrain files from fish-feed
    
    Refs: #63

[33mcommit 667cab30485f53b7d2d58e9b8a9598d8301049dc[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Apr 12 11:44:43 2023 +0200

    fix: pull from fish-feed
    
    Refs: #63

[33mcommit 696f020741736299af4907ce2906b314b2a35e5b[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Apr 12 11:42:16 2023 +0200

    fix: override max size to allow 8k textures
    
    Refs: #63

[33mcommit 68d62448f70a83f61ab3e7acc711e97d2bf99d5c[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 12 11:38:35 2023 +0200

    feat: make everything in the scene baked

[33mcommit 0fa7b1941c5d423c6cdb980456b464d0c5083016[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Apr 12 11:36:09 2023 +0200

    feat: remodel office and dock
    
    new office model, and a complete prefab called "OfficeComplete"
    
    Refs: #63

[33mcommit 036bfde072d4f4f3ae653231eaf7e79a43547083[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Apr 12 10:53:10 2023 +0200

    feat: baked lighting

[33mcommit 7b857afd62895f11d0b1bbf7d279094565f2ec8b[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Apr 12 10:43:58 2023 +0200

    fix: merdcontrols funciton as expected

[33mcommit ddf82342241a9da0949dac4cfe0dab795785f173[m
Merge: 58c6df02 e2d6e98d
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Wed Apr 12 09:47:10 2023 +0200

    Merge pull request #82 from vr4vet/underwater-scenery
    
    feat: add underwater environment; fix bugs related to camera inputs

[33mcommit 58c6df02c6cf525991227f48a010ba6fc721f26b[m
Merge: 648e9752 b5eb7ba5
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Tue Apr 11 20:38:52 2023 +0200

    Merge pull request #81 from vr4vet/improve-fish-behaviour
    
    Improve fish behaviour

[33mcommit e2d6e98d628e7b0785085ba6977c148cea72cd91[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Tue Apr 11 19:14:08 2023 +0200

    feat: add underwater environment; fix bugs related to camera inputs

[33mcommit b5eb7ba59e7c12538c94f4f2381b9154c9bffbcf[m
Merge: 95d3b217 648e9752
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Apr 11 19:10:11 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into improve-fish-behaviour

[33mcommit 95d3b21744172b843ee4335c221ff19e1fbce068[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Apr 11 18:49:04 2023 +0200

    feat/fix: random fish and stop killing when idle
    
    implemented 10 fish that swim randomly without getting hungry or dying.
    
    Refs: #65

[33mcommit 49eaac6add4d675d781027003e18b18fa4b86526[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Apr 11 18:08:30 2023 +0200

    feat/fix: fish behaviour and bug fixes
    
    fish system now positions itself logically where the 3d cursor is placed
    in the scene. this includes the particle system, which now also adjusts
    its position and radius accordingly. fish also moved horisontally using
    the parameter in the fish system prefab, while at the same time not
    tilting too much.
    
    Refs: #65

[33mcommit 648e9752d9e3866cc31bf3758cd2ef903f2e3d2b[m
Merge: 3b7ed7f5 8eff4b5b
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Tue Apr 11 14:41:58 2023 +0200

    Merge pull request #79 from vr4vet/improve-start-game
    
    Improve start game

[33mcommit 8eff4b5ba1c4ee31e2e69809d7d8db27c84315ed[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Apr 11 14:33:46 2023 +0200

    fix: fix activation zone with info box
    
    Tried improving how the player is activated when they enter the
    activation zone. The info box is for now always shown to the user if
    they are not playing the game.
    
    Refs: #53

[33mcommit b3e232e8490a353c6c1d8b9fbf550a523f615061[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Apr 11 13:23:29 2023 +0200

    feat: separate game logic from score script
    
    Refactored scoring-script and created a new script, Game. Which contains
    start logic, timer, and hud-logic for showing text on the merd screen.
    Have also tried fixing the activation logic.
    
    Refs: #70, #53

[33mcommit adda0d94a3d8c52087f3bba3190f906c7d6d6cce[m
Merge: 46cfe151 3b7ed7f5
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Apr 11 12:02:22 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into improve-fish-behaviour

[33mcommit 46cfe151638beb0878c3ef4a1918c53808a9b198[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Mon Apr 10 09:52:45 2023 +0200

    fix: shift position of fish sprite and improve swimming
    
    less aggressive pitching when swimming up or down, and shift the
    position of the sprite so it lines up with the 3D model, making the
    transition more seamless.
    
    Refs: #65

[33mcommit 75fc35b7e7980d69d230565cdfb9f0457252fac2[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 7 14:35:23 2023 +0200

    feat: add infobox for starting game
    
    Added an infobox that tells the user "Press A to start game" when they
    are in the activation zone.
    
    Refs: #53

[33mcommit 3b7ed7f5dab58ae1d1fb357a0830b992e2af2d0c[m
Merge: 3cfa57c4 68d8a5eb
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Fri Apr 7 12:18:15 2023 +0200

    Merge pull request #77 from vr4vet/switch-level-tablet
    
    Switch level tablet

[33mcommit 68d8a5eb6973d59d16fd25c795211a3fb555f018[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Apr 7 12:07:20 2023 +0200

    fix: fix scene showing glow for activation area
    
    Refs: #53

[33mcommit 3e65f6c0cc6075fbd5dde1fcc002b4889d261541[m
Merge: d50841cc 71a5ca40
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Apr 4 15:57:25 2023 +0200

    Merge branch 'switch-level-tablet' of https://github.com/vr4vet/Blue-Sector into switch-level-tablet

[33mcommit d50841cc2db95d70fc571b665ff007f4ce0127cd[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Apr 4 15:55:25 2023 +0200

    fix: fix glow material for activation area
    
    Refs: #53

[33mcommit 71a5ca4013ff0ed89bfb83fbaaef70b7599bdff9[m
Merge: 424245d8 ad436408
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Apr 4 14:52:00 2023 +0200

    Merge branch 'switch-level-tablet' of https://github.com/vr4vet/Blue-Sector into switch-level-tablet

[33mcommit 424245d808a319237dcda4e048487b27d78874bc[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Apr 4 14:51:18 2023 +0200

    fix: pointer issue

[33mcommit 384faca15eb2ad47173cc4007fc87029a7a81847[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Apr 4 14:42:38 2023 +0200

    feat: made upwards and downwards shifting less aggressive
    
    not done yet
    
    Refs: #65

[33mcommit ad4364081f2e1325a01d637956f5b752ca7f207b[m
Merge: 1c8b22ad 3cfa57c4
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Apr 4 14:41:35 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into switch-level-tablet

[33mcommit 1c8b22ad56d4086216d25fdd82ce2567566f4b56[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Apr 4 13:58:18 2023 +0200

    fix: change the scene to only contain buttons for levels
    
    Changed LevelsAndStart scene to Levels and to only contain buttons for
    levels.
    
    Refs: #54

[33mcommit 70eab5d5254dd443cee36fca122e20c179016866[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Tue Apr 4 13:44:21 2023 +0200

    chore: made prefabs and improved file structure
    
    Added the canvas prefab containing the HUD to the MerdCameraHost prefab.
    Moved prefabs related to scoring to the Scoring folder.
    
    Refs: #68

[33mcommit 3cfa57c41a534471432cba3e5bdab28a4a8b82bc[m
Merge: 8c823326 fbd42496
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Mon Apr 3 19:44:48 2023 +0200

    Merge pull request #74 from vr4vet/fix-and-reimplement-idle-state-logic
    
    Fix and reimplement idle state logic

[33mcommit fbd42496f5940e9ae632e6b634b581924551b520[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Mon Apr 3 18:37:09 2023 +0200

    fix: reimplement idle state logic
    
    implements idle state as another state in the FishState enum, and
    includes related updates/additions to logic in FishSystemScript and FishScript.
    
    also adjusts code in SlideBridge, as the game otherwise will not
    compile. There's no guarantee that this script behaves as originally
    intended, but it didn't seem to be finished at this time. It should now
    set feedingIntensity properly in FishSystemScript, but I have not tested this.
    
    Refs: #73

[33mcommit c4a8a6f837cee6a93c47a4ece68b9c88e77631df[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Mon Apr 3 13:24:26 2023 +0200

    fix: pointer issue

[33mcommit 8c823326001e49d09b6856bdd0dc2aa9ca20dd31[m
Merge: 02017f46 a6c51c5d
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 16:41:38 2023 +0200

    Merge pull request #60 from vr4vet/level-load-refactor
    
    Level load refactor

[33mcommit a6c51c5d88a419d374d8859fc4c32af45915c6b1[m
Merge: a13cc9cb 02017f46
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 16:41:22 2023 +0200

    Merge branch 'fish-feed' into level-load-refactor

[33mcommit b1f0679fddcf3f92e6e344f7fd400685b5c4166e[m
Merge: af8d24cc 879dcbb4
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 16:37:43 2023 +0200

    Merge pull request #55 from vr4vet/conventional-commit-CI
    
    Merge branch conventional-commit-CI

[33mcommit 02017f4668b583fe3f1b34cb74b9c5dca31ce9a0[m
Merge: 93578d5e 53c7f38e
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 16:37:19 2023 +0200

    Merge pull request #62 from vr4vet/fish-state
    
    feat: implement fish idle state (wip)

[33mcommit 53c7f38eae010122a89983ed9f55e6086da56294[m
Merge: 054bb75f 93578d5e
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 16:36:00 2023 +0200

    Merge branch 'fish-feed' into fish-state

[33mcommit a13cc9cbebf5f74fc34293de273b8bbcfd78d621[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 16:29:07 2023 +0200

    feat: add sandbox mode

[33mcommit 985ec899c8566d52600b12e28f3df8833f564494[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 15:26:19 2023 +0200

    refactor: load modes instead of levels

[33mcommit 6768048ab1fdd88b4807e6af0e1023cd3fe35f8b[m
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Fri Mar 31 15:25:57 2023 +0200

    fix: added white glow material and tried fixing HUD
    
    Refs: #33

[33mcommit 2bbe58adf97487f52797b5ad71adf3b5c2488d1c[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 31 15:22:55 2023 +0200

    feat: implement fish feed slider

[33mcommit 93578d5e4fda2547ab9bda9975da4ff54f04bdb8[m
Merge: 6e5742a2 448be74b
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Mar 31 15:02:43 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into fish-feed

[33mcommit 6e5742a22e2b88f3b6337e30060ebc6e8e02573a[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Mar 31 15:01:44 2023 +0200

    Misc: add cylinder for darkness around fish cage

[33mcommit 448be74bc0347e4d35fb2c9a1696fb71d1f9160a[m
Merge: c63139f4 7f117631
Author: TrymPet <trym2001@hotmail.com>
Date:   Fri Mar 31 14:44:13 2023 +0200

    Merge pull request #44 from vr4vet/basic-tutorial
    
    Basic tutorial

[33mcommit 7f1176317c4cb478b3d2c9a02372532c90ce7ccb[m
Merge: 60ffd03d c63139f4
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 31 14:42:37 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into basic-tutorial

[33mcommit c63139f482f0bec8f5dda6cadabc28f05b73aa15[m
Merge: 44ea391d 0a1addb2
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Fri Mar 31 14:41:10 2023 +0200

    Merge pull request #59 from vr4vet/lod-on-fish
    
    Lod on fish

[33mcommit 0a1addb262e3c6a9b8bf4616adaf3503fd7b62ae[m
Merge: 0dc6f19a 44ea391d
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Mar 31 14:36:50 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into lod-on-fish

[33mcommit 0dc6f19a37d3916fd78d43331db505390b745e27[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Fri Mar 31 14:30:56 2023 +0200

    fix: pointer stuff in VR4VET folder
    
    Refs: #39

[33mcommit 44ea391d9163656e150a14bcfb23641c344414d5[m
Merge: 5a0f540e 375dadc1
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Fri Mar 31 14:26:19 2023 +0200

    Merge pull request #58 from vr4vet/office-environments
    
    Office environments

[33mcommit 5e022ab0b3b37a41f053ba38dcc70c26af8171d5[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Mar 31 13:33:38 2023 +0200

    fix: changed the FishFeedingWithHUD scene
    
    Refs: #33

[33mcommit d8975fa344a44938d2a6b6c637ec5ab6d9bc56d0[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Mar 31 13:29:51 2023 +0200

    feat: combined HUD with merdmonitor
    
    Added screen visualization with the camera and fishsystem. Food wastage
    and dead fish still needs to be correctly shown.
    
    Refs: #33

[33mcommit e695ebd53a3a65d493a158c6943d0cce4dfe9d72[m
Merge: 84f4de47 5a0f540e
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Mar 31 09:55:58 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into switch-level-tablet

[33mcommit 84f4de474258ffa72be7e0af11fff3a5c1f398f7[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Fri Mar 31 09:49:31 2023 +0200

    feat: add activation area and game start with button A
    
    Added an activation area the user have to stand in to start the game and
    starts the game by pressing the A button. Still needs an infobox that
    informs the user how to start the game.
    
    Refs: #53

[33mcommit 60ffd03d77504ef9c0ef5409af7f2628e7cb0c71[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 18:52:06 2023 +0200

    feat: Hints are disabled in realism mode #52

[33mcommit b1033a8aca813559c84081532c5829ed58ae637e[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 18:30:07 2023 +0200

    choire: make the sign look better

[33mcommit 3f6f5ef4ab07262591ea9f152f2e8ed79f8f7bdc[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 18:22:47 2023 +0200

    choire: fix merge conflict

[33mcommit 162525f0f1b217f76aee0461d2f3546013266d4d[m
Merge: b9675992 5a0f540e
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 18:21:27 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into basic-tutorial

[33mcommit 5a0f540e05e0a2885efe1d9f818f54342774df76[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 18:16:01 2023 +0200

    choire: update placeholder merd to use fish system

[33mcommit b96759929faf3b676a9bc5eedd317442d0958c2d[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 18:01:17 2023 +0200

    Place tutorial/hints in the scene

[33mcommit deb3be081d2e12fb27df67000b7a0294b218a972[m
Merge: c767b290 c84bf5c4
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 17:15:22 2023 +0200

    Merge remote-tracking branch 'origin/camera-integration' into basic-tutorial

[33mcommit c767b290e93e1ec9d876e763270a815b6fa6693b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 17:15:00 2023 +0200

    tmp

[33mcommit 4705c6f39e95435448b0889c5d39245dce4d6b4f[m
Merge: c58691b8 c84bf5c4
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Thu Mar 30 15:59:28 2023 +0200

    Merge pull request #57 from vr4vet/camera-integration
    
    Add fish system integration

[33mcommit ad53dd0c3d2ef060d14969f3660c5de61fd68d60[m
Merge: 82580d80 c58691b8
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 15:26:41 2023 +0200

    Merge remote-tracking branch 'origin/fish-feed' into basic-tutorial

[33mcommit 82580d801a0c8bd7cc297a3da1cae7d51e0b61ed[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 15:22:57 2023 +0200

    add remainder of tutorial/hints

[33mcommit c84bf5c4e3bff0a48fc6d4e6458d16c2b0094ebc[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 30 13:17:32 2023 +0200

    add fish system integration

[33mcommit 28c2ca7dc91839bf484798a5672f70eaadd2bd40[m
Author: Quynh-Lan Nguyen Pham <quynhlanp@gmail.com>
Date:   Thu Mar 30 12:47:33 2023 +0200

    feat: add buttons to switch levels
    
    Added buttons the user can click to switch levels and a simple script
    that changes a temporary UI text depending on the button clicked.
    
    Refs: #54

[33mcommit 83b7ebd2d593be98083e4b09ba77eb527f60ab12[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Mar 29 22:23:34 2023 +0200

    feat: add lod sprite
    
    The fish fbx is difficult, and I could not find a way to modify the 3D
    model at this time, but a sprite at quite close distances actually looks
    kinda convincing as long as the amount of fish is sufficiently high.
    Poly count mostly stays below 90-100k even wih 1200 fish. Culling
    at huge distances helps as well.
    
    Refs: #39

[33mcommit 375dadc17baf317921c2761b33769e3985576b1b[m
Merge: 4025755f c58691b8
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Mar 29 19:55:45 2023 +0200

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into office-environments

[33mcommit 4025755f371c6d3b7aced65e90e406304f172c84[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Mar 29 19:29:58 2023 +0200

    fix: repair VR4VET asset folder after I broke it
    
    Copy-paste and replace the content of the VR4VET folder into this branch
    to fix issues after I removed some unused assets that incidently broke
    the XR player
    
    Refs: #6

[33mcommit 802f4b94b000a545c6d1004ad47008ff0c6dde61[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Mar 29 19:10:25 2023 +0200

    feat: add new dock, building, all kinds of furniture
    
    some of the models are from the 2018 project, while others are modelled
    from scratch. All are retextured and/or UV-unwrapped and using manipulating 2018 textures made in Gimp
    
    Refs: #6

[33mcommit 71c456d266d72db7317a72f892e5f11c5591f449[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Sun Mar 26 16:06:09 2023 +0200

    feat: add some furniture and start modelling house
    
    Refs: #6

[33mcommit c58691b8758de924dcf0d708fc09baa67f1308fd[m
Merge: d7f4347d 56cf03f7
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Sat Mar 25 13:33:07 2023 +0100

    Merge pull request #51 from vr4vet/update-scoring-and-hud
    
    Update scoring and hud

[33mcommit 879dcbb49fc786bdb6e5263dcc8453cf56b5b8f8[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 21:34:14 2023 +0100

    ci: add caching of dependencies
    
    Adds caching of dependencies to make runtime significantly shorter.
    (note: only works on 2. run onwards).
    Also made so workflow only runs on (all) pull requests and pushes (only)
    main and develop
    
    refs: #55 #20

[33mcommit fedeeb7e903eb898538c832c667bb7cf5a365277[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 17:13:40 2023 +0100

    fix: fix typo in workflow

[33mcommit a7b14680bda9200a7382ef168a4b11f0596fe9ee[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 17:09:56 2023 +0100

    fix(ci): update path for node dependencies
    
    refs: #55 #20

[33mcommit 46ae46f5886c389b05e956c45ac1e10662726f8f[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 16:56:27 2023 +0100

    perf(ci): use caching of node packages for perf
    
    Moved to using the actions 'setup-node' and 'cache' to hopefully improve
    ci speed and cut down on un-needed traffic.
    Also added required dependencies in .github folder, as well as updating
    .gitignore to reflect this.
    
    refs: #50 #20 #11

[33mcommit 13836068408aa22a97b587da999ac462ad8aa4dd[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 14:31:21 2023 +0100

    fix: install needed npm-packages during workflow
    
    added 'npm install @commitlint/config-conventional' to ensure all needed
    npm packages are installed.
    
    refs: #55 #20 #11

[33mcommit 0303f11e48b9390ca371e9cba54dea5c7907a2f6[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 14:24:51 2023 +0100

    fix: add commitlint config to workflow
    
    added creation of commitlint configfile to workflow.
    
    refs: #55 #20 #11

[33mcommit 5e5e214cf885637b36ba1fc2641a77d07b69b506[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 14:12:50 2023 +0100

    fix: add sudo to elevate permission
    
    added sudo to install lines of workflow script to stop permission denied
    error.
    
    refs: #55 #20 #11

[33mcommit 3d69fffb28c56ab60dc67dd6fa65fd4f1da66f2d[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 13:53:03 2023 +0100

    chore: move pr template to .github folder
    
    moved pr template to .github folder to clean up repo structure.
    
    refs: #11

[33mcommit 4472adb4ae6d23dd085d9dfe52976cb881261a1f[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 24 13:46:02 2023 +0100

    ci: create conventionalcommits workflow
    
    added ci workflow for conventional commits to make sure commits follow
    the standard.
    
    refs: #20

[33mcommit 56cf03f7572b35c4ab0cda301fd4741ad179170d[m
Merge: 28c06325 d7f4347d
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Fri Mar 24 10:46:04 2023 +0100

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into update-scoring-and-hud

[33mcommit 28c063255e34bec27b07416474088e9d131971bb[m
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Fri Mar 24 10:36:40 2023 +0100

    feat: implement visualization of stats on screen
    
    The montitor shows the time left in the game, current score, food
    waste per second and the amount of dead fish.
    
    Refs: Closes #33

[33mcommit 054bb75fd814fea69a48164b2e48667a1da2ee80[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Wed Mar 22 16:59:25 2023 +0100

    feat: implement fish idle state (wip)
    
    implemented fish idle state, although some parts of DoD remain.
    
    refs: #38

[33mcommit d7f4347dd95ce77d4a7e24f7696501fec1932a0d[m
Merge: edde3d6b f8a4e2e5
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Wed Mar 22 15:01:30 2023 +0100

    merge: merge levels into fish feed
    
    merge levels into fish feed to more easily resolve conflicts
    
    refs: #27

[33mcommit 1dacfda7f1f86c0c75baf1780673607c964aa4d4[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Mar 22 12:45:13 2023 +0100

    add tutorial hints

[33mcommit 270800373a727a2ee3c89e7500ec25a0f9d78d94[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Wed Mar 22 12:44:41 2023 +0100

    finish tutorial popup

[33mcommit 198b6116e1307c05a4bd8474e375445ac1d33a5d[m
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Wed Mar 22 12:20:21 2023 +0100

    feat: Update score calculation to be based on merd
    
    Updated score logic so it calculates based on the school of fish rather
    than each individual fish.
    
    Refs: Closes #36

[33mcommit a57944323892405ff731332bac2da340e18b6ee8[m
Merge: a2a3c7ac edde3d6b
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Tue Mar 21 20:34:38 2023 +0100

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into update-scoring-and-hud

[33mcommit a2a3c7acd840589cb862da6577de3868ec5df354[m
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Tue Mar 21 20:28:30 2023 +0100

    feat: Added monitors and score on screen
    
    Added monitors and text on screen. The score is now shown on the screen
    after the game ends.
    
    Refs: #33

[33mcommit edde3d6be836e3d6c30e068057e3d92302a4615d[m
Merge: f4ef9966 46246859
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Tue Mar 21 19:41:06 2023 +0100

    Merge pull request #48 from vr4vet/update-fish-system
    
    Update fish system

[33mcommit 46246859fe75adfc76a448663a0d28d0c632fe0a[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Mar 21 19:29:06 2023 +0100

    chore: pull the newst from fish-feed
    
    Update this branch. It has the newest from the fish-feed branch
    
    Refs: #35

[33mcommit f6f46b7dac0fc182b67053b29fb4a82b3c5dbf90[m
Merge: 5460cf73 f4ef9966
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Mar 21 19:26:06 2023 +0100

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into update-fish-system

[33mcommit 5460cf73f57458ecd8705296d1c9857bed2a219d[m
Merge: cec916d6 46d597fc
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Mar 21 19:20:05 2023 +0100

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into update-fish-system

[33mcommit f4ef9966b80e28734746e342559c3d8c9ddd6cc5[m
Merge: 5e600d90 ad9d8abc
Author: TrymPet <trym2001@hotmail.com>
Date:   Tue Mar 21 19:01:44 2023 +0100

    Merge pull request #42 from vr4vet/add-camera-prefab
    
    Add camera prefab

[33mcommit 5e600d909f312f077979f4277c0b2f6446ec7696[m
Merge: 46d597fc 96518ff2
Author: TrymPet <trym2001@hotmail.com>
Date:   Tue Mar 21 19:01:36 2023 +0100

    Merge pull request #43 from vr4vet/create-fish-feed-scene
    
    Create fish feed scene.

[33mcommit 96518ff2c9799bf002ad2c84b7fc4964703c5677[m
Merge: 4af6158e 46d597fc
Author: TrymPet <trym2001@hotmail.com>
Date:   Tue Mar 21 19:01:15 2023 +0100

    Merge branch 'fish-feed' into create-fish-feed-scene

[33mcommit cec916d6491441fe773171fe9c705c9e2709463d[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Mar 21 18:35:18 2023 +0100

    chore: removing unused variables
    
    Refs: #35

[33mcommit 4ee462b003a48c1eb15bd74d6c52c98ed9ef9ae6[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Mar 21 18:27:12 2023 +0100

    Fix: fish also swim upwards if starving
    
    fish should also swim upwards if starving, and not just when hungry.
    
    Refs: #35

[33mcommit ab5080f62fac3640ac4283a0c02596698d7e05d7[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Mar 21 16:14:07 2023 +0100

    feat: add global fish state in fish system
    
    Fish do not have their own hunger state, and position themselves
    uniformally when the state is changed betweeen Full and Hungry/Dying.
    
    A new fish dies every 5 sec when in Dying state.
    
    Refs: #35

[33mcommit 46d597fcc2c35019b88399b592e9db3755882a41[m
Merge: 596eb57c 57728687
Author: eamathie <69533149+eamathie@users.noreply.github.com>
Date:   Mon Mar 20 15:29:33 2023 +0100

    Merge pull request #46 from vr4vet/scoring
    
    Scoring

[33mcommit 57728687b329af7ba2e48ad15a9c113d79402d63[m
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Mon Mar 20 15:23:01 2023 +0100

    fix: Moved the scoring to the fish system scene
    
    Moved the scoring, added some comments and cleaned the code.
    
    Refs: #26

[33mcommit 09d6abea2f2f147af55a1360ad9db5e89f6be946[m
Merge: 9e15703f 596eb57c
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Mon Mar 20 13:39:19 2023 +0100

    Merge branch 'fish-feed' of https://github.com/vr4vet/Blue-Sector into scoring

[33mcommit 596eb57ca01d16959f2a4cb8dec7703254960b85[m
Merge: c4978a8f 26f51a00
Author: qlpham231 <69513958+qlpham231@users.noreply.github.com>
Date:   Mon Mar 20 12:11:11 2023 +0100

    Merge pull request #34 from vr4vet/fish-system
    
    Fish system

[33mcommit 26f51a002d494f7c6a70710c491a08cdf0d22953[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Mon Mar 20 11:58:59 2023 +0100

    fix: make particle system trigger and clean project
    
    Remove unused assets and folders used for testing. Particles fixed by
    unchecking Play On Awake.
    
    Refs: #32

[33mcommit 46705aaa82cbd2f71c4bf187cb726dcbb4046b54[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 17 21:37:40 2023 +0100

    create rounded rect shader

[33mcommit a2ed0811f3610a68da36c5002685a8f509ef37c0[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 17 21:37:28 2023 +0100

    create popup script

[33mcommit c12165cc3f37f817395faf67435db6202a092376[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 17 21:36:33 2023 +0100

    create tutorial popup

[33mcommit ad9d8abc656ebeb4a37011c6884f9b21be003add[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 17 19:29:52 2023 +0100

    delete old, unused assets

[33mcommit 4af6158e8e35038c7a9ec94cffeeb89c4f00f727[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 17 19:18:27 2023 +0100

    add basic scene

[33mcommit f8a4e2e5d5c794cc5a7e2882e006e69aab967456[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 17 19:11:10 2023 +0100

    refactor: move loading of levels
    
    moved loading of levels to 'LevelLoader'.
    still needs some tweaking, but should work.
    
    refs: #27

[33mcommit 35b4695e46410b85643cda31007f882d45e63575[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 17 18:17:35 2023 +0100

    finalize merd camera

[33mcommit e886007f1f372ea953f6614fc913e4ffe44bb469[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 17 18:01:10 2023 +0100

    feat: load level data from xml (WIP)
    
    refs: #27

[33mcommit ca3f3f47e1d19254e33b729e9217210d5da5cacb[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 17 17:32:21 2023 +0100

    feat: add level-loader (WIP)
    
    refs: #27

[33mcommit e59470dc6cb9b212acc2e8c49d5d8a8f2e4b27a2[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 17 11:22:43 2023 +0100

    Finish camera

[33mcommit 9e15703ff9d971c60ba70a38b341124f6f54c212[m
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Fri Mar 17 10:46:38 2023 +0100

    feat: Added scoring based on food wastage
    
    The score takes into consideration how much food is wasted in addition to if the fish is Full, Hungry or Dead.
    
    Refs: #26

[33mcommit 3b66b1efaae6f986885d369cd929e5f954d4c93d[m
Merge: af8d24cc 55c6032e
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 17 09:42:33 2023 +0100

    Merge remote-tracking branch 'origin/scoring' into levels

[33mcommit 22f31a73373ca10b2a3b08f77ad0220dfcb3557d[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Thu Mar 16 11:30:12 2023 +0100

    tmp

[33mcommit f7248a930f845ee9e4b34e2ba1b2a5093c33a0ae[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Mar 15 15:23:48 2023 +0100

    fix: make multiple fish systems work at the same time
    
    Position of fish is relative to the fish system it was instansiated by.
    
    Speed of fish and other parameters are adjusted to prevent slow
    movement.
    
    Refs: #32

[33mcommit af8d24ccd3f51bcc855b2fa07acf085bb5d8623d[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Mar 15 15:19:23 2023 +0100

    docs: update README
    
    update readme to include tutorial on using gitmessage template
    
    refs: #19 #20

[33mcommit 1e66ce6cb3a4455152b34c1114557c57e03cb1c1[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Mar 15 15:10:19 2023 +0100

    chore(repo): update gitmessage

[33mcommit f22de4b7de77e24d12937cd028a80ce096b71377[m
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Mar 15 15:09:07 2023 +0100

    chore(repo): update gitmessage template
    
    update .gitmessage template to more completely explain the conventional commits specification

[33mcommit b82523e668cb5858915fc337a86bc265d1d4979d[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Wed Mar 15 11:32:56 2023 +0100

    feat: Added fish model, visualisation of system, fixed food particles
    
    optional body: Changed some variables to make them clearer, and the fish
    system is now more customisable via the editor insted of having to
    change the code

[33mcommit 55c6032e5a4cdff2d84fc023eae9a9604efbb484[m
Merge: 0950f751 99d2c9a0
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Wed Mar 15 10:43:38 2023 +0100

    Merge branch 'fish-system' of https://github.com/vr4vet/Blue-Sector into scoring

[33mcommit 0950f7518360f0eee71d95b226c8d0bd42765dc0[m
Author: Quynh-Lan Nguyen Pham <qlpham@stud.ntnu.no>
Date:   Tue Mar 14 11:23:39 2023 +0100

    feat: Added script for scoring based on fish performance|
    
    optional body: Gives a score based on fish performance in a time period.

[33mcommit 99d2c9a0e414d9693517a8140af950334dee35b4[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Tue Mar 14 11:03:09 2023 +0100

    feat: implemented feeding system and relative postioning

[33mcommit 9ceaafaeef5dac29cb4754a8d11fa70f39b406dd[m
Author: Emil Aron Andresen Mathiesen <eamathie@stud.ntnu.no>
Date:   Mon Mar 13 12:20:08 2023 +0100

    feat: fish system where fish can swim and move according to hunger
    
    optional body: fish move up when hungry. There are parameters to decide
    amount of fish and so on.
    
    Co-authored-by: Lan quynhlanp@gmail.com

[33mcommit db4d3e29001cb55d75b27ddb80e75d8a464fdae9[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 10 14:42:08 2023 +0100

    chore(repo): update README to outline needed asset

[33mcommit 18f2967648d2abe2c568591d0ec6180b09b1769e[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 10 14:39:56 2023 +0100

    chore(repo): update gitignore and removed files

[33mcommit 52972c8c025631c2b66af750327132c898aada7f[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 10 13:37:33 2023 +0100

    chore(repo): remove ignored files

[33mcommit e0f2ba729a11e0ff34093cd67ac3aad4197fbe36[m
Merge: afc31f42 4b4108ab
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Fri Mar 10 13:23:50 2023 +0100

    Merge pull request #30 from vr4vet/add-readme
    
    Add basic instructions for setting up the project

[33mcommit 4b4108ab3a7124fc98a84e026550d04e863e288a[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Fri Mar 10 13:00:04 2023 +0100

    (Choire) Add basic instructions for setting up the project

[33mcommit afc31f4211987f4d8bc147aea564e06a4bb5e040[m
Merge: c4978a8f fa502ee8
Author: TrymPet <trym2001@hotmail.com>
Date:   Fri Mar 10 13:18:32 2023 +0100

    Merge pull request #31 from vr4vet/git-config
    
    chore(repo): update gitignore & gitattributes

[33mcommit fa502ee83636922099a45bb45b2397fd33e5dec8[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Mar 10 12:58:34 2023 +0100

    chore(repo): update gitignore

[33mcommit c4978a8fe0d7d699a82647b12e9d10d6f5597e28[m
Merge: a6df39df a2ed7f84
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Mar 8 15:01:05 2023 +0100

    Merge pull request #29 from vr4vet/fish-feed
    
    WIP: Port 2018 fish scenario

[33mcommit a2ed7f8489312f67c1cce0961f3b0d8982b5b7a0[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Tue Mar 7 14:44:08 2023 +0100

    add merd

[33mcommit e7b74c46383b23f3c8e7f69be4bf9362d77ce09b[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Mar 6 23:16:44 2023 +0100

    add control house

[33mcommit a270ff742455a77872113febc405a9eb4841b402[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Mar 6 21:45:54 2023 +0100

    cleanup

[33mcommit b3bae27e342b5dd31f1d9d5e211e2df14a588c2a[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Mar 6 21:12:23 2023 +0100

    add ocean

[33mcommit a6df39df03f00512c5e7f1507809981d73d20a29[m
Merge: 526de056 18956245
Author: aleksjoh <s0554907@htw-berlin.de>
Date:   Wed Mar 1 12:55:18 2023 +0100

    Merge branch 'fish-welfare' into develop

[33mcommit 18956245531b225f023ebb0cddef5f311521b181[m
Author: MagnusBau <magnus.baugerd@hotmail.com>
Date:   Tue Feb 28 17:33:29 2023 +0100

    Add task competion when putting fish in tank

[33mcommit 64d8320e8cdbc5de5d5872bea5ac2a2480e5b5a2[m
Author: MagnusBau <magnus.baugerd@hotmail.com>
Date:   Tue Feb 28 16:55:45 2023 +0100

    Remove 3d models from gitignore

[33mcommit c84a50113d1ebe0901e81e8fdc09a6db175bfcfa[m
Author: MagnusBau <magnus.baugerd@hotmail.com>
Date:   Tue Feb 28 16:54:36 2023 +0100

    Remove 3d models from gitignore

[33mcommit 1ab6801fc4f9f2122447fa731f169516fc451cb0[m
Author: MagnusBau <magnus.baugerd@hotmail.com>
Date:   Tue Feb 28 16:17:55 2023 +0100

    Add tank and control panel to welfare scene

[33mcommit ea0d69737b9608f28c365a6a8d2697c174af2d17[m
Author: MagnusBau <magnus.baugerd@hotmail.com>
Date:   Mon Feb 27 13:12:00 2023 +0100

    Add tasks to 'fish welfare scene

[33mcommit 8718f24d240793923806ea3e8eea4447b781e95e[m
Author: MagnusBau <magnus.baugerd@hotmail.com>
Date:   Fri Feb 24 17:45:16 2023 +0100

    Work on fishwelfarescene

[33mcommit 58c3d0407537aa40cd2cb20fffd59e584b359ef6[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Feb 24 14:18:25 2023 +0100

    chore(repo): update gitignore

[33mcommit 3d6a302c423002fc5ae0fa55cdb83abbf755d06f[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Fri Feb 24 14:15:40 2023 +0100

    chore(repo): update gitattributes

[33mcommit a20aec17117201afd8dc2af45dd9161185e1baf0[m
Author: magnubau <magnus.baugerd@hotmail.com>
Date:   Thu Feb 23 15:55:44 2023 +0100

    Add statesaver and textmesh pro

[33mcommit 526de056f686ce22464f7510420e84e6ec51bff9[m
Merge: db0e2b38 0fbf7e33
Author: TrymPet <trym2001@hotmail.com>
Date:   Thu Feb 23 14:24:42 2023 +0100

    Merge pull request #18 from vr4vet/pr-template
    
    chore(repo): create pull request template

[33mcommit 567aa252ba24d954856e06753f3cfe3818137b0b[m
Author: magnubau <magnus.baugerd@hotmail.com>
Date:   Thu Feb 23 13:48:46 2023 +0100

    Add bng framework to gitignore

[33mcommit ffc6409d891864a896f2f378cdf60dc74f26fc0b[m
Author: magnubau <magnus.baugerd@hotmail.com>
Date:   Thu Feb 23 13:48:09 2023 +0100

    Add user settings to gitignore

[33mcommit 0fbf7e33f0ef709853c1e47e761e5bc41b9d3635[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Wed Feb 22 12:31:47 2023 +0100

    chore(repo): create pull request template
    
    Created pull request template for repo, inspired by:
    https://axolo.co/blog/p/part-3-github-pull-request-template
    
    Refs: #11

[33mcommit db0e2b385ed098328e2150c1da6811d7fd6c9cf3[m
Merge: 308087e0 628b06ac
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Feb 22 14:32:12 2023 +0100

    Merge pull request #16 from vr4vet/repo-setup
    
    new project from VR template

[33mcommit e49dfd824de869fd2b2e61f4d8259d0baa943fd3[m
Merge: 147dbf00 9971219e
Author: Tor Jacob Neple <62819835+torjacob@users.noreply.github.com>
Date:   Wed Feb 22 14:32:12 2023 +0100

    Merge pull request #16 from vr4vet/repo-setup
    
    new project from VR template

[33mcommit 308087e0fe70ab329e5586275d288cf8d8528a73[m
Merge: 68a3c178 786d05cc
Author: TrymPet <trym2001@hotmail.com>
Date:   Wed Feb 22 14:31:55 2023 +0100

    Merge pull request #17 from vr4vet/commit-template
    
    chore(repo): set up commit message template

[33mcommit 147dbf00729d26a80fa3a364809b3ad5946864ab[m
Merge: 0b29c612 4905d4aa
Author: TrymPet <trym2001@hotmail.com>
Date:   Wed Feb 22 14:31:55 2023 +0100

    Merge pull request #17 from vr4vet/commit-template
    
    chore(repo): set up commit message template

[33mcommit 786d05cc95ec20956cb45e273e5c0cf339d19e74[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Mon Feb 20 14:04:35 2023 +0100

    chore(repo): set up commit message template
    
    Set up commit message template to make it easier for team members to
    follow agreed upon specification for commits.
    
    To make sure this template is being used, execute this command in the
    root of the repository:
    'git config commit.template .gitmessage'
    
    NB! This template will be overridden if the -m flag is used when
    preforming a git commit.
    
    For more information about the template/specification, see here:
    https://www.conventionalcommits.org/
    
    Refs: #15

[33mcommit 4905d4aae81029093f79dd31c63ee73a99ca4767[m
Author: Tor Jacob Neple <torjne@stud.ntnu.no>
Date:   Mon Feb 20 14:04:35 2023 +0100

    chore(repo): set up commit message template
    
    Set up commit message template to make it easier for team members to
    follow agreed upon specification for commits.
    
    To make sure this template is being used, execute this command in the
    root of the repository:
    'git config commit.template .gitmessage'
    
    NB! This template will be overridden if the -m flag is used when
    preforming a git commit.
    
    For more information about the template/specification, see here:
    https://www.conventionalcommits.org/
    
    Refs: #15

[33mcommit 628b06ac32cfb8c18296c84b5f6a24c7a9c1c404[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Feb 20 14:12:03 2023 +0100

    import VR4VET package

[33mcommit 9971219eacfe16cb021a6bbfd6223ba3457c1613[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Feb 20 14:12:03 2023 +0100

    import VR4VET package

[33mcommit ccd837dad98f005cbc9375abfbdc78af8bacd8f9[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Feb 20 14:09:55 2023 +0100

    new project from VR template

[33mcommit f9c2bcaae710d595013e42c71aa43ddccf97c629[m
Author: Trym Lund Flogard <trym@flogard.no>
Date:   Mon Feb 20 14:09:55 2023 +0100

    new project from VR template

[33mcommit 68a3c178a73c5376708d77e039e96de96903c42f[m
Author: Mikhail Fominykh <mihail.fominyh@gmail.com>
Date:   Fri Jan 20 15:28:29 2023 +0100

    Initial commit

[33mcommit 0b29c6125b6e4e4f647da317fb600d3884c01e28[m
Author: Mikhail Fominykh <mihail.fominyh@gmail.com>
Date:   Fri Jan 20 15:28:29 2023 +0100

    Initial commit

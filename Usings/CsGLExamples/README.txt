
CsGL Examples 0.9.0 BETA
October 13, 2002


***************************************************************************
***                          BETA NOTICE                                ***
***                                                                     ***
***  This is a beta release for the CsGL community and the original     ***
***  authors of these examples to review and offer any suggestions      ***
***  before the public 1.0.0 release.                                   ***
***                                                                     ***
***  Currently, NeHe lessons 13-15 (the font lessons) are missing due   ***
***  to issues with the wgl font methods, as well as, NeHe lessons      ***
***  30-32, 35, and 38s which I just haven't gotten to yet.             ***
***  38 and 38e work but are not being compiled, they're supposed to    ***
***  have their resources compiled into the exe, but as of now that's   ***
***  not being done, pending some work on an update of ResBuilder.      ***
***                                                                     ***
***  Some of the Redbook examples are missing, I was doing them in      ***
***  alphabetical order, so the ones towards the bottom of the          ***
***  alphabet are missing.  I'll get them done soon.  Some things to    ***
***  note about some of the Redbook examples.  The ones using color     ***
***  index mode, aren't actually using color index mode, the examples   ***
***  using the accumulation buffer are unbelievably slow, the GLUT      ***
***  shapes are also very slow, and the *hedron is corrupted, a few of  ***
***  the examples throw an exception on minimization.  I'm working      ***
***  with Lloyd to get these issues corrected.  Also, I'm using a       ***
***  somewhat modified CsGL, which added these GLUT shapes and          ***
***  some new safe wrappers, these changes will be included in the      ***
***  next release, once these issues have been corrected.  It's hard    ***
***  for Lloyd and I to stay in sync, but we will get things in order,  ***
***  hopefully before the next CsGL release, and definitely before the  ***
***  public release of these examples.                                  ***
***                                                                     ***
***  Hopefully, the font issues will get resolved.  The other missing   ***
***  lessons will be finished before the public release.  Also, a       ***
***  simple launcher will be made for running these examples from a     ***
***  single interface, providing pictures and descriptions, and saved   ***
***  preferences rather than having to launch each executable blindly.  ***
***                                                                     ***
***  Also I experience issues with NeHe lesson 24 which presents as     ***
***  trouble on my Radeon causing a crash.  As such, I've included      ***
***  NeHeLesson24a which somewhat works around this issue.  I could     ***
***  use help testing these two lesson 24's.  Apparently, for whatever  ***
***  reason, glGetString returns corrupt GL_VERSION and GL_RENDERER     ***
***  strings anytime after the first call on my Radeon.  I do not see   ***
***  this on my Nvidia card.  Lesson 24 also loads the TGA very slowly  ***
***  I'll look into this before public release.                         ***
***                                                                     ***
***  There are also a few other issues of which I'm aware and am going  ***
***  to correct before 1.0.0 goes out, however I'd still like your      ***
***  feedback, code contributions, opinions, and insights.  Contact me  ***
***  through email or the forum.  I'd rather have them now than after   ***
***  1.0.0 goes out, so let me know if you see an issue or have a fix.  ***
***                                                                     ***
***  If you think you can get some useful examples or ports of          ***
***  examples done in the next couple weeks (vague estimate for 1.0.0)  ***
***  let me know and I'll try to get them included.  Bonus points if    ***
***  the example is from a scheduled set of examples as per the         ***
***  roadmap listed further down (Mesa, GLUT, GameTutorials, Nate       ***
***  Miller's, Sulaco)...                                               ***
***                                                                     ***
***  Read this file for more information and areas where you can help   ***
***  correct an issue or offer insights.  I'd like to get many of       ***
***  these things corrected or understood before the public release.    ***
***                                                                     ***
***                        READ THIS FILE!                              ***
***************************************************************************


--- INTRODUCTION ---

These are some examples using CsGL, OpenGL .NET, available from 
http://csgl.sourceforge.net/.  The purpose for these examples are to 
educate those interested in developing OpenGL applications for the .Net 
Framework, utilizing CsGL, and to help improve CsGL by exposing any problems 
or weaknesses that it may have.

Currently over 100 examples, including:
 - A small basecode layer, for quickly allowing the developer to produce 
   OpenGL applications using CsGL
 - All of the Redbook examples, 
   http://www.opengl.org/developers/code/examples/redbook/
 - All of the NeHe OpenGL lessons, plus a few extra, courtesy of NeHe, 
   http://nehe.gamedev.net/
 - Six examples from Robert Schaap, 
   http://vulcanus.its.tudelft.nl/robert/opengl.html
 - Pong Mania courtesy of Steve Wortham,
   http://www.gldomain.com/Programs/PongMania.htm
 - A few others, some from upcoming CsGLExamples versions.

Most of the examples have differing functionality based on user input, hit 
F1 to get a listing of the available functionality.

These are ports to C#, while most are well commented, if you'd like to 
know the theory behind the examples, please refer to the original sites 
for this information, if available.


--- RUNNING THE EXAMPLES ---

To run these examples you'll need to make sure you have the .Net Framework, 
available at http://msdn.microsoft.com/netframework/ or from Windows Update.

You should also take this time to make sure you're running the latest 
Service Pack for the .Net Framework (currently Service Pack 2).  These
examples are built against the 1.0 release of the .Net Framework
(with SP2).

Finally, it'd be most useful to have a good OpenGL card/driver combo, check 
with your video card manufacturer for the latest driver, however, if all 
else fails most of these examples should run with the software OpenGL
implementation (albeit very slowly).


--- FUTURE ---

In the future we plan on doing more nifty examples, including:
 - Tracking any new NeHe lessons.
 - The Mesa3D demos from http://www.mesa3d.org/
 - The GLUT examples from GLUT 3.7.6
 - The OpenGL tutorials from 
   http://www.gametutorials.com/Tutorials/OpenGL/OpenGL_Pg1.htm
 - The Sulaco OpenGL examples from http://www.sulaco.co.za/opengl.htm
 - Possibly some from Nate Miller from http://nate.scuzzy.net/programming/
 - Any original examples we can come up with on our own
 - A few others here and there

However, we'd always like suggestions on what you'd like to see included, 
if you have some ideas, suggestions, links to good examples, or better yet, 
some examples you've written yourself and would like to see included, 
please let us know (contact info below).

We also plan for CsGL (and these examples) to run on non-Windows platforms.  
We believe this won't be a problem and have planned accordingly.  These 
should run on Mono, http://www.go-mono.com/ once Windows.Forms is properly 
supported.  When this happens, we'll update the examples accordingly.  So 
if you'd like to see .Net and CsGL working on Linux (and other platforms) 
help out the Mono team.

See the roadmap section below.


--- FEEDBACK ---

If you have any problems, difficulties, or encounter "undocumented features" 
with these examples, please let us know.  Be sure to include which 
example gave you problems, the make, model, and driver version of your 
video card, what platform you're on, and any additional information that 
might be valuable in tracking down the problem.

Our biggest request is for feedback on the code in the examples and moreso 
for the basecode.  We'd love to make the code cleaner and faster, so if 
you have some useful suggestions, or better yet, some code changes, send 
them our way.  We're sure there's room for improvement speed-wise, so if 
you're an optimization guru or know any .Net performance enhancements 
we're missing, please let us know (especially Microsoft employees, .Net 
Framework know-it-alls, and OpenGL gurus). Help us to improve the basecode,
offer suggestions or contribute code!  We'd like to improve the speed,
functionality, and robustness of the basecode to help make OpenGL
development using CsGL as simple and robust as possible.

Good places to start for performance improvements would be: NeHe Lessons 
11, 19, 34, 36, and 37, as well as the Schaap examples.  Array and 
floating point performance would likely be areas to concentrate on.  Also, 
memory usage could be improved, give us a hand minimizing the working set. 

Error handling could also be improved, as well as, fully commenting the 
non-NeHe lessons.  Help us out.

If you're wondering why the executables are so large, approximately 34KB 
of each executable is due to the embedded icon, which contains icons at 
16x16, 32x32, 48x48, each at 4bit, 8bit, and 24bit color depths.  But, boy 
does it ever look snazzy.  :)

Finally, we'd love to have people submit these examples in other .Net 
languages, so if you're up to porting these examples to VB.Net, J#, COBOL 
for .Net, Perl for .Net, etc., send them our way.  There will likely be 
CLS compliance issues to be resolved, they will be dealt with in 
association with the particular port contributors.


--- BUILDING THE EXAMPLES ---

These examples assume you have some knowledge of C#, the .Net Framework, 
and ideally, OpenGL.  We suggest you read the source code thoroughly. 
It may also be of use to compare these C# ports to their originals to 
see the required changes for using C#, the .Net Framework, CsGL, and 
the CsGL basecode.  Also, read the known issues (as listed below) for 
special information regarding these examples.

To build these examples you'll first need the .Net Framework SDK (see 
above link), but you'll also want to get NAnt, which we use for the
build process, from http://nant.sourceforge.net/.  You'll want to put 
NAnt somewhere in your environment's path and you'll want to verify 
that csc (the C# compiler) is also in your path.

Versions used for this release:
 - .Net Framework 1.0 Service Pack 2
 - CsGL 1.4.0 (with some small additions)
 - nant-snapshot-20020930

Once you have the archive unzipped (retaining directory structure), 
drop to the command line (make sure NAnt and csc are in your path), 
navigate to the CsGLExamples directory (the directory this Readme.txt 
is in), you should also see CsGLExamples.build in this directory, this 
is our build script for NAnt.  Once you're there in your command prompt 
type: "nant -projecthelp".  This command will output the available build 
targets included in the project.  You can then type: "nant targetname", 
where targetname is the name of the target action you wish to perform.  
For instance, "nant build-debug" will build the debug binaries, while 
"nant build-release" would build the release binaries.  This should get 
you on your way.  Improvements on the build process are gladly accepted.

There are currently three dependencies for building these examples (besides 
what's listed above), first is CsGL.dll, which, of course, is the .Net 
wrapper for OpenGL.  This assembly is provided in both release and debug 
forms.  The second is CsGL.Native.dll which is the native code piece of 
the CsGL wrapper.  The third is ResBuilder.exe, which generates .resource 
files from a text description and any objects you want to include, such as 
bitmaps, icons, etc.  CsGL.dll, CsGL.Native.dll, and ResBuilder were 
developed by The CsGL Development Team, and you can get the source for 
these from our website, if you so choose.  The source for these has not 
been included here, only the binaries, due to the already large size of 
this archive. 

We suggest Visual Studio .NET http://msdn.microsoft.com/vstudio/ or 
SharpDevelop http://www.icsharpcode.net/OpenSource/SD/ as your IDE.  We use 
a lot of #region blocks (which, at last check, are not supported in 
SharpDevelop).  We also have inline code comments set pretty wide (I have 
a big monitor and run at a high resolution).  We also use indenting and 
smart tabs, with a size of 4.  These things may make the code more difficult 
for you to read, we apologize for any inconvenience.  However, we've done 
our best to make all of the code self-consistant.  We have not included 
project files since we wanted to appeal to the widest audience, however, if 
you'd like to contribute projects/solutions (for Visual Studio) or 
projects/combines (for SharpDevelop), we'd be glad to take them.

If you can't seem to figure out how to debug without having a project, you 
should use FrameworkSDK\GuiDebug\DbgCLR.exe.  You'll find this where you've 
installed the .Net Framework.

We've attempted to make the basecode framework as extensible as possible, 
you should be able to extend or override just about anything you need by 
overriding the appropriate methods or properties.  A good place to start, 
besides examining the examples themselves, is Model.cs, the base of all 
the examples.  Refer to the basecode documentation or the basecode source 
itself.  Unfortunately, the basecode has become a bit unwieldy, hopefully, 
we'll clean it up (with your help and suggestions) before the next major 
release.  We'll endeavor to keep the interface for inheriting from Model 
the same, so anything you write will require little to no changes.  
However, it'd be most useful to clean up the structure of the basecode, 
there's a lot of excess that could be trimmed down, so if you're a whiz 
with those crazy UML tools, lend a hand with the basecode.


--- KNOWN ISSUES ---

If you can help fix or give suggestions or feedback on these issues, 
please do so.

I should note some oddities involving the working set.  When I load up 
an example and watch the memory usage in task manager I'll see the 
example using about 14MB and about 8MB in the VM.  When the example is 
minimized it'll drop to about 2.6MB and 8MB, then when restored it'll be 
using around 3.6MB and 8MB.  This is just one of the examples, but all 
Windows Forms I've tested (including Visual Studio) exhibit this behavior 
so I'm a little confused on what's being dropped on minimization.  This 
appears to be normal behavior for .Net applications.  If someone could 
enlighten me, I'd appreciate it.

NeHe lessons 13-15 are currently not working.  We need to find a way to 
deal with fonts in CsGL.  We apparently cannot make use of the wgl font
methods, due to some strangeness with contexts.

The Redbook examples involving color index mode aren't actually using 
color index mode, I haven't explored color index mode and CsGL as of yet.

The examples using the accumulation buffer are so unbelievably slow it 
hurts.  I'm not sure what the problem is yet, I'll have to explore, but 
something is definitely wrong with CsGL and the accumulation buffer, they 
can't be that slow...

Note, the Redbook examples were not compared to the original executables, 
some differences may exist.  I quickly did these from their original source 
and didn't bother compiling the native versions for comparison.

The Schaap Fire doesn't use the mouse to move the flame (it uses the
keyboard), I was too lazy to come up with the proper translation of 
mouse and viewport position. Schaap made use of DirectX which makes this
easy.  I also did not implement the same Windows Form as he did for his
Plasma example, instead the keyboard will have to suffice.

The movement in most of these examples is framerate dependent, this is 
how the originals were, so that's how these are.  So if things move in 
a wild and spastic manner, this is the reason.  There's nothing to 
prevent you from implementing framerate independent movement, however.
Also, in some of the examples using GLUT (such as the Redbook examples)
they make use of the idle callback in GLUT.  I don't have much experience
with GLUT to know exactly how this method functions, so in some of these
examples I threw in a Thread.Sleep to slow down animation enough to be
viewable.  If your framerate on some of these examples stays the same it's
either because you have Wait For V/Sync enabled in your driver, or I'm using
Thread.Sleep, which is a bit of a hack...  Also of note, when there were
any differences between any examples as per their web pages and the code
itself, we've followed the code.

Covering the main window (including the fullscreen window), even the 
slightest bit with another window, including the Help screen, severely 
affects framerate.  Usually, you wouldn't do this, however, the Help screen 
is provided for informational purposes.  In most cases, the fullscreen mode 
will achieve higher framerates than windowed mode, however, if you check 
the framerate in fullscreen mode by utilizing the Help screen, framerate 
will be drastically reduced.

The basecode's Help screen has a number of issues, relating to the 
DataGrid.  The implementation of the DataGrid presented here is overly 
complex for what we were wanting to accomplish.  Essentially, we wanted 
a display-only DataGrid, with scrolling and row highlighting only.  
However, there's a lot of complications implementing this.  Just setting 
the DataGrid as ReadOnly doesn't accomplish this.  Many related events 
set the CurrentCell, such as scrolling and hitting the tab key, so these 
events had to be overridden to prevent this.  To further complicate things 
there's no good way to set the CurrentCell to nothing, so instead we call 
the CancelEdit() method.  Perhaps someone could send us a better way of 
going about this.

We would've liked to deny selections in the RichTextBox as well, however, 
we found no good way of accomplishing this either.  Suggestions welcomed.  

You'll also notice the visual style of the vertical scroll bar in the 
DataGrid does not match that of the RichTextBox (at least on Windows XP), 
this is another oddity.  

In the examples involving texturing, NeHe makes use of GL_RGB to specify 
the texture's image data, however, we use GL_BGR_EXT or GL_BGRA_EXT.  He 
uses the glaux library to load his images, which properly return data for 
OpenGL in RGB format.  We're using .Net's Bitmap and LockBits methods, 
however, despite their names, the enumerations PixelFormat.Format32bppArgb 
and Format24bppRgb, etc., when used in a Bitmap.LockBits method do not 
return image data in ARGB or RGB like one might expect, but as BGRA or 
BGR.  While we're aware Windows internally uses BGR, these PixelFormats, 
by their naming, seem somewhat illogical.  This is obviously for historical 
endian reasons, but it'd be nice to have some "proper" RGB PixelFormats.  
This really shouldn't be a problem, but in case you're wondering, this is 
why we're doing it so.  If anyone could give us a clue here, we'd 
appreciate it.

Also, be sure to note that .Net uses Unicode chars/strings, so often when 
dealing with porting C/C++ OpenGL code using fonts, be sure to specify 
GL_UNSIGNED_SHORT rather than GL_UNSIGNED_BYTE.  Be mindful of other type 
differences as well when porting native code.


--- THANKS ---

We'd like to thank the following people:

Lloyd Dupont, CsGL Lead Developer, http://dev.galador.net/ 
Jeff Molofee, NeHe Productions, http://nehe.gamedev.net/ 
Anyone who assisted on or wrote one of the original NeHe lessons 
Robert Schaap, http://vulcanus.its.tudelft.nl/robert/
Mark J. Kilgard, Nvidia, GLUT Fame
Brian Paul, Mesa3D, http://www.mesa3d.org/
Steve Wortham, http://www.gldomain.com/
The OpenGL Architecture Review Board 
Silicon Graphics, Inc.
Microsoft Corporation 


--- HISTORY ---

Version 0.9.0 BETA - October 13, 2002:
    Initial Beta Release For CsGL Community.


--- ROAD MAP ---

There is no timeline for these releases.  The more help and contributions 
I get, the sooner they get released.  Some space has been left for 
additional releases, if needed.  These are major milestones, there's 
a lot of examples to be added in each of these milestones.

1.0.0 - Any fixes, additions, or optimizations to the 0.9.0 release.
        Completion of missing lessons/examples.  A simple app launcher.
        First public release.

1.2.0 - Any fixes, additions, or optimizations to the 1.0.0 release.
        All of the Mesa3D and GLUT examples.

1.4.0 - Any fixes, additions, or optimizations to the 1.2.0 release.
        All of the GameTutorials.com OpenGL tutorials.

1.6.0 - Any fixes, additions, or optimizations to the 1.4.0 release.
        All of the Sulaco OpenGL examples.



--- USEFUL LINKS ---

CsGL Main Site         http://csgl.sourceforge.net/ 
GotDotNet              http://www.gotdotnet.com/ 
GotDotNet Workspaces   http://www.gotdotnet.com/community/workspaces/
Mentalis Multimedia    http://www.mentalis.org/soft/class.qpx?id=1 
Mesa3D                 http://mesa3d.sourceforge.net/ 
Mono                   http://www.go-mono.com/ 
NAnt                   http://nant.sourceforge.net/ 
nBASS                  http://www.codeproject.com/useritems/nBASS.asp 
NDOC                   http://ndoc.sourceforge.net/ 
NeHe Productions       http://nehe.gamedev.net/ 
.Net                   http://www.microsoft.com/net/ 
.Net Framework         http://msdn.microsoft.com/netframework/ 
OpenGL                 http://www.opengl.org/ 
SharpDevelop           http://www.icsharpcode.net/OpenSource/SD/
SourceForge            http://sourceforge.net/ 
Visual Studio .NET     http://msdn.microsoft.com/vstudio/ 
Windows Forms FAQ      http://www.syncfusion.com/faq/winforms/ 


--- AUTHORS ---

Randy Ridge      CsGL Examples Lead Developer   randyridge@hotmail.com 
Amir Ghezelbash  CsGL Examples Contributor      amir-20@rogers.com 
Lloyd Dupont     CsGL Lead Developer            lloyd@galador.net 


Send us an email or visit our forum.

Make sure to include CsGL in the subject line for any email, lest you wish 
your important email to be deleted amongst the spam.



HW
==

A homework tracker and organizer. I made this a long time ago, and I didn't know about source control at the time, or even how to write mantainable programs, apparently. The source code for this is a mess.

Versions
========

I think the version trouble came about because I tried to add new features but broke the existing ones in the process, and I didn't know about version control at the time, so I wrote over the originial source.

* NetbookBin/
    * This directory has the most orignial - and least broken - executable. I believe the source for it is gone.
* V2/
    * This directory was an attempt to add more features, but for some reason involving multithreading with windows it doesn't work. I've pretty much given up on this version.
* HW/
    * This directory has the most recent working version. I was able to take the source from a working verison before V2, and add the features I wanted while not causing the threading problem (not really sure how). However, this version is *extremely* buggy, so I still use the version in NetbookBin when I use this program. With a major refactoring and a lot of testing, this could surpass the NetbookBin version though.

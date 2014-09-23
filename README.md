mise-demo
=========

This Maritime Information Sharing Environment (MISE) GitHub site is an
unofficial 3rd party demo. See the [MISE website](https://mise.mda.gov/)
for the official source material.

The MISE may be understood in terms of a 
"[common profile](http://pi2.ida.org/common-profile)" whereby a single
reference view is expanded on by technical guidance views, each of which may
have sub-views, either providing further technical guidance or details about
specific implementations:

![MISE Common Profile Overview](MISE.png)

This GitHub project shares some of the implementation-level code:

* [Java](https://github.com/ida-org/mise-demo/tree/master/MDA-clients/java) -
    - The 
      [source jar](https://github.com/ida-org/mise-demo/blob/master/MDA-clients/java/MDAUtils-1.0-SNAPSHOT-sources.jar) and 
      [binary jar](https://github.com/ida-org/mise-demo/blob/master/MDA-clients/java/MDAUtils-1.0-SNAPSHOT-jar-with-dependencies.jar)
      contains all of the implementation plumbing for
      handling the various details laid oud in the Security and Data technical
      guidance views and various subviews.
    - For how to access the publish service (including deleting resources), as 
      well as the retrieve service, and search service (not in diagram), see 
      the [service client example programs](https://github.com/ida-org/mise-demo/tree/master/MDA-clients/java/src/test).
* [.Net (C#)](https://github.com/ida-org/mise-demo/tree/master/MDA-clients/dotnet) -
  In particular, see the
  [service client sample code](https://github.com/ida-org/mise-demo/blob/master/MDA-clients/dotnet/ClientTest/ClientTest.cs).

Downloads:

* [GitHub ZIP](https://github.com/ida-org/mise-demo/archive/master.zip)
* [Official Packages](https://mise.mda.gov/drupal/tools)
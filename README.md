mise-demo
=========

This Maritime Information Sharing Environment (MISE) GitHub site is an
unofficial 3rd party demo. See the [MISE website](https://mise.mda.gov/)
for the official source material.

## Common Profile

The MISE may be understood in terms of a 
"[common profile](http://pi2.ida.org/common-profile)" whereby a single
reference view is expanded on by technical guidance views, each of which may
have sub-views, either providing further technical guidance or details about
specific implementations:

![MISE Common Profile Overview](MISE.png)

This GitHub project shares some of the implementation-level code. The 
"Implementation View Plumbing" refers to code that provides the various
requirements laid out in the Security and Data technical guidance views and 
various subviews. The "Client Sample Code" shows how to access the publish
service (including deleting resources), as well as the retrieve service, and 
search service (not in diagram).

| Implementation | Implementation View Plumbing | Client Sample Code|
| ---------------|------------------------------|-------------------|
| [Java](https://github.com/ida-org/mise-demo/tree/master/MDA-clients/java) | source and binary jar files - <code>MDA-clients/java/MDAUtils-1.0-*.jar</code> | [MDA-clients/java/src/test](https://github.com/ida-org/mise-demo/tree/master/MDA-clients/java/src/test) |
| [.Net (C#)](https://github.com/ida-org/mise-demo/tree/master/MDA-clients/dotnet) | [MDA-clients/dotnet/MdaToolkit] (https://github.com/ida-org/mise-demo/tree/master/MDA-clients/dotnet/MdaToolkit) | [MDA-clients/dotnet/ClientTest/ClientTest.cs](https://github.com/ida-org/mise-demo/blob/master/MDA-clients/dotnet/ClientTest/ClientTest.cs) |

## Downloads

* [GitHub ZIP](https://github.com/ida-org/mise-demo/archive/master.zip)
* [Official Packages](https://mise.mda.gov/drupal/tools)
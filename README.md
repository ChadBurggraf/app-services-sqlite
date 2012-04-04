# System.ApplicationServices.SQLite
#### ASP.NET Membership, Role and Profile providers for SQLite.

This is a re-packaging of the SQLite application services providers by Roger Martin.
The original source and accompanying information can be found at
<http://www.codeproject.com/Articles/29199/SQLite-Membership-Role-and-Profile-Providers>.

## Building

To build a signed, release version of `System.ApplicationServices.SQLite.dll`, you need
to generate a signing key and place it in the `Source` directory:

    sn -k Source\System.ApplicationServices.SQLite.snk

Next, run the build script with MSBuild:

	MSBuild Build.proj

The output will appear in the `Build` directory, under the repository root. To check
that your assembly was successfully signed, run the following command under the 
`Build` directory:

	sn -Tp Build\System.ApplicationServices.SQLite.dll

If a public key is reported, the assembly was successfully signed.

## Usage

  1. Install [System.Data.SQLite.dll](http://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki) 
     on your machine, or bin-deploy the appropriate platform/framework version in your application.
  2. Add a reference to System.ApplicationServices.SQLite.dll in your application.
  3. Configure the relevant providers and connection strings. See the [original CodeProject
     article](http://www.codeproject.com/Articles/29199/SQLite-Membership-Role-and-Profile-Providers)
	 for detailed information.

## License

Licensed under the [CPOL](http://www.codeproject.com/info/cpol10.aspx) license. See LICENSE.txt.

Copyright (c) 2008, Roger Martin.
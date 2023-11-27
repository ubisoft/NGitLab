// This is needed for quick migration from NUnit 3.x -> 4.x
// https://docs.nunit.org/articles/nunit/release-notes/Nunit4.0-MigrationGuide.html#use-global-using-aliases
global using Assert = NUnit.Framework.Legacy.ClassicAssert;
global using CollectionAssert = NUnit.Framework.Legacy.CollectionAssert;
global using StringAssert = NUnit.Framework.Legacy.StringAssert;

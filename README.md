# ContourToUmbracoFormsMigrator
Migrate your Contour forms to Umbraco Forms forms

# Please Note
This fork has been hacked around to use a cloned copy of Umbraco Datalayer to allow access to the old
(Contour) database from Umbraco version 7.6 onwards. From this version the Umbraco SQLHelper no longer
allows access to an alternative database (the API is still there, but broken), and ContourToUmbracoFormsMigrator
relies on this functionality. More information here: 
https://our.umbraco.org/projects/developer-tools/umbraco-forms-migration/computer-says-no/60252-SQL-helper-exception

This fork is _completely unsupported_ - use at your own risk.

# Limitations
1. As it stands at the moment, this version of ContourToUmbracoFormsMigrator will import the forms themselves but 
_not associated data (form submissions)_ - I didn't need to keep these so never investigated this.
2. This version does not fix problems importing from older versions of Contour that exist in the main
distribution of ContourToUmbracoFormsMigrator. If you are running an old version you are still likely to
need to upgrade it first.

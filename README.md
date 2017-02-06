# stockscanner
A scanner to determine how presence in headlines reflects on that day's stock changes.

For set up, create app.config (excluded on purpose) to create a connection string to a database (SQL diagram included). Also include the following within the ocnfiguration tag:

  <appSettings>
    <add key="NyTimesApiKey" value="<key>" />
    <add key="EmailAddress" value="<email>" />
    <add key="EmailPassword" value="<password>" />
    <add key="EmailHost" value="<email host>" />
  </appSettings>
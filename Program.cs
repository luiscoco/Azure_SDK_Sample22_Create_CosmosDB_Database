using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;

//1. Obtaine Azure credentials
TokenCredential cred = new DefaultAzureCredential();

//2. Azure Authentication
ArmClient client = new ArmClient(cred);

//3. Set the Azure SubscriptionId, the ResourceGroupName where the Azure CosmosDB is located and the Azure CosmosDB account name
string subscriptionId = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
string resourceGroupName = "rg1";
string accountName = "newcosmosdbwithazuresdk";

//4. We create the ResourceGroup identifier
ResourceIdentifier cosmosDBAccountResourceId = CosmosDBAccountResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, accountName);

//5. We get the CosmosDB Account Resource inside the ResourceGroup
CosmosDBAccountResource cosmosDBAccount = client.GetCosmosDBAccountResource(cosmosDBAccountResourceId);

//6. We get the database in the ComosDB account
CosmosDBSqlDatabaseCollection collection = cosmosDBAccount.GetCosmosDBSqlDatabases();

//7. We set the parameters for creating a new database in the CosmosDB account
string databaseName = "ToDoList";
CosmosDBSqlDatabaseCreateOrUpdateContent content = new CosmosDBSqlDatabaseCreateOrUpdateContent(new AzureLocation("WestEurope"), new CosmosDBSqlDatabaseResourceInfo(databaseName))
{
    Options = new CosmosDBCreateUpdateConfig(),
    Tags =
{
},
};

//8. We create a new database in the CosmosDB account
ArmOperation<CosmosDBSqlDatabaseResource> lro = await collection.CreateOrUpdateAsync(WaitUntil.Completed, databaseName, content);

//9. We get the Database resource
CosmosDBSqlDatabaseResource result = lro.Value;

//10. We get the Database data
CosmosDBSqlDatabaseData resourceData = result.Data;

//11. We pring the Database Id
Console.WriteLine($"Succeeded on id: {resourceData.Id}");




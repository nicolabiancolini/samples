param suffix string = uniqueString(newGuid())

param azureBlobConnectionName string = 'azureblob${suffix}'
param azureEventGridConnectionName string = 'eventgrid${suffix}'
param appInsightsName string = 'ai${suffix}'
param storageAccountName string = 'stgacc${suffix}'
param readCustomerIdentityDetailsLogicAppName string = 'read-customer-identity-details-la${suffix}'
param appInsightsActionGroupName string = 'Application Insights Smart Detection'
param customerIdentityDetailsFillerLogicAppName string = 'customer-identity-details-filler-la${suffix}'
param appInsightsWorkspaceResourceId string

var customerContainerName = 'customers'

resource appInsightsActionGroup 'microsoft.insights/actionGroups@2019-06-01' = {
  name: appInsightsActionGroupName
  location: 'Global'
  properties: {
    groupShortName: 'SmartDetect'
    enabled: true
    armRoleReceivers: [
      {
        name: 'Monitoring Contributor'
        roleId: '749f88d5-cbae-40b8-bcfc-e573ddc772fa'
        useCommonAlertSchema: true
      }
      {
        name: 'Monitoring Reader'
        roleId: '43d0d8ad-25c7-4714-9337-8ba259a9fe05'
        useCommonAlertSchema: true
      }
    ]
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: resourceGroup().location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 90
    WorkspaceResourceId: appInsightsWorkspaceResourceId
    IngestionMode: 'LogAnalytics'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource customerIdentityDetailsFillerLogicApp 'Microsoft.Logic/workflows@2019-05-01' = {
  name: customerIdentityDetailsFillerLogicAppName
  location: resourceGroup().location
  properties: {
    state: 'Enabled'
    definition: {
      '$schema': 'https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#'
      contentVersion: '1.0.0.0'
      parameters: {}
      triggers: {}
      actions: {}
      outputs: {}
    }
    parameters: {}
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: storageAccountName
  location: resourceGroup().location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    defaultToOAuthAuthentication: false
    allowCrossTenantReplication: true
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: true
    allowSharedKeyAccess: true
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
}

resource customerContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-06-01' = {
  name: '${storageAccount.name}/default/${customerContainerName}'
}

resource azureEventGridConnection 'Microsoft.Web/connections@2016-06-01' = {
  name: azureEventGridConnectionName
  location: resourceGroup().location
  properties: {
    displayName: azureEventGridConnectionName
    statuses: [
      {
        status: 'Connected'
      }
    ]
    customParameterValues: {}
    nonSecretParameterValues: {
      'token:TenantId': tenant().tenantId
      'token:grantType': 'code'
    }
    api: {
      name: azureEventGridConnectionName
      displayName: 'Azure Event Grid'
      description: 'Azure Event Grid is an eventing backplane that enables event based programing with pub/sub semantics and reliable distribution & delivery for all services in Azure as well as third parties.'
      type: 'Microsoft.Web/locations/managedApis'
    }
    testLinks: []
  }
}

resource readCustomerIdentityDetailsLogicApp 'Microsoft.Logic/workflows@2019-05-01' = {
  name: readCustomerIdentityDetailsLogicAppName
  location: resourceGroup().location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    state: 'Enabled'
    definition: {
      '$schema': 'https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#'
      contentVersion: '1.0.0.0'
      parameters: {
        '$connections': {
          defaultValue: {}
          type: 'Object'
        }
      }
      triggers: {
        manual: {
          type: 'Request'
          kind: 'Http'
          inputs: {
            method: 'GET'
            relativePath: 'customers/{Id}'
            schema: {}
          }
        }
      }
      actions: {
        'Get_blob_content_(V2)': {
          runAfter: {}
          type: 'ApiConnection'
          inputs: {
            host: {
              connection: {
                name: '@parameters(\'$connections\')[\'azureblob\'][\'connectionId\']'
              }
            }
            method: 'get'
            path: '/v2/datasets/@{encodeURIComponent(encodeURIComponent(\'${storageAccount.name}\'))}/files/@{encodeURIComponent(encodeURIComponent(\'${customerContainerName}/\',triggerOutputs()[\'relativePathParameters\'][\'Id\']))}/content'
            queries: {
              inferContentType: true
            }
          }
        }
        Response: {
          runAfter: {
            Validate_blob_content: [
              'Succeeded'
            ]
          }
          type: 'Response'
          kind: 'Http'
          inputs: {
            body: '@body(\'Validate_blob_content\')'
            statusCode: 200
          }
        }
        Validate_blob_content: {
          runAfter: {
            'Get_blob_content_(V2)': [
              'Succeeded'
            ]
          }
          type: 'ParseJson'
          inputs: {
            content: '@base64ToString(body(\'Get_blob_content_(V2)\')[\'$content\'])'
            schema: {
              properties: {
                location: {
                  type: 'string'
                }
                'work-teams': {
                  items: {
                    type: 'string'
                  }
                  type: 'array'
                }
              }
              type: 'object'
            }
          }
        }
      }
      outputs: {}
    }
    parameters: {
      '$connections': {
        value: {
          azureblob: {
            connectionId: azureBlobConnection.id
            connectionName: 'azureblob'
            connectionProperties: {
              authentication: {
                type: 'ManagedServiceIdentity'
              }
            }
          }
        }
      }
    }
  }
}

resource azureBlobConnection 'Microsoft.Web/connections@2016-06-01' = {
  name: azureBlobConnectionName
  location: resourceGroup().location
  properties: {
    displayName: 'https://${storageAccountName}.blob.${environment().suffixes.storage}/${customerContainerName}'
    statuses: [
      {
        status: 'Ready'
      }
    ]
    api: {
      name: azureBlobConnectionName
      displayName: 'Azure Blob Storage'
      description: 'Microsoft Azure Storage provides a massively scalable, durable, and highly available storage for data on the cloud, and serves as the data storage solution for modern applications. Connect to Blob Storage to perform various operations such as create, update, get and delete on blobs in your Azure Storage account.'
    }
    testLinks: [
      {
        requestUri: 'https://${environment().authentication.audiences[1]}:443/subscriptions/${subscription().id}/resourceGroups/${resourceGroup().name}/providers/Microsoft.Web/connections/${azureBlobConnectionName}/extensions/proxy/testconnection?api-version=2016-06-01'
        method: 'get'
      }
    ]
  }
  dependsOn: [
    storageAccount
  ]
}

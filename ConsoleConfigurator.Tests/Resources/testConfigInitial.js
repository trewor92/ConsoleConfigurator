const env = require('@elateral/config-module');
const { searchQueryType } = require('./common/constants');

const config = {
  graphql: {
    disableTracing: env('GRAPHQL_DISABLE_TRACING').default(true).bool().exec(),
  },
  searchResult: {
    max_attribute_count: env('MAX_ATTRIBUTE_COUNT').default(1000).exec(),
  },
  env: env('NODE_ENV').default('production').exec(),
  log: {
    level: env('LOG_LEVEL').default('DEBUG').exec(),
  },
  mongodb: {
    uri: env('MONGO_URI').default('mongodb://localhost:27017/').exec(),
  },
  elastic: {
    uri: env('ELASTIC_URI').default('localhost:9200').exec(),
    log: env('ELASTIC_LOG_LEVEL').default('error').exec(),
    reindex: {
      pageSize: env('REINDEX_PAGE_SIZE').default(25).exec(),
      chunkSize: env('REINDEX_CHUNK_SIZE').default(2500).exec(),
    },
    searchQueryType: env('ELASTIC_SEARCH_QUERY_TYPE').default(searchQueryType.AUTO).exec(),
    indices: {
      content: {
        aliasName: env('ELASTIC_CONTENT_INDEX').default('eio-%s-contentitems').exec(),
        entities: {
          asset: env('ELASTIC_ASSET_INDEX').default('eio-%s-asset-v%s').exec(),
          campaign: env('ELASTIC_CAMPAIGN_INDEX').default('eio-%s-campaign-v%s').exec(),
        },
      },
      // user: {
      //   aliasName: env('ELASTIC_USER_INDEX').default('eio-%s-users').exec(),
      //   entities: {
      //     user: env('ELASTIC_USER_INDEX').default('eio-%s-user-idx').exec(),
      //   }
      // }
    },
  },
  port: env('PORT').default(8088).int().exec(),
  messageQueue: {
    host: env('AZURE_SERVICEBUS_HOST').default('elateral-test-bus').exec(),
    sasKeyName: env('SASKEY_NAME').default('RootManageSharedAccessKey').exec(),
    sasKey: env('SASKEY').default('someBusKeyHere').exec(),
    topic: env('AZURE_SERVICEBUS_TOPIC').default('elateral.io.topic').exec(),
    priorityRange: env('QUEUE_MESSAGE_PRIORITY').exec(),
    serviceName: env('SERVICE_NAME').default('search-service').exec(),
  },
  service: {
    asset: env('ASSET_SERVICE_URL').default('http://localhost:8086').exec(),
    account: env('ACCOUNT_SERVICE_URL').default('http://localhost:8082').exec(),
    campaign: env('CAMPAIGN_SERVICE_URL').default('http://localhost:8084').exec(),
    // user: env('USER_SERVICE_URL').default('http://localhost:8083').exec(),
    attributeDefinition: env('ATTRIBUTE_DEFINITION_SERVICE_URL').default('http://localhost:8092').exec(),
    authorization: env('AUTHORIZATION_SERVICE_URL').default('http://localhost:8081').exec(),
    translation: env('TRANSLATION_SERVICE_URL').default('http://localhost:8098').exec(),
  },
  dependencies: {
    skipRuntimeChecks: env('DEPENDENCIES_SKIP_RUNTIME_CHECKS').default(true).bool().exec(),
    skipDbVersionChecks: env('DEPENDENCIES_SKIP_DBVERSION_CHECKS').default(true).bool().exec(),
    attributeDefinition: {
      url: env('ATTRIBUTE_DEFINITION_SERVICE_URL').default('http://localhost:8092').exec(),
      runtime: { minVersion: '0.0.1' },
    },
    authorization: {
      url: env('AUTHORIZATION_SERVICE_URL').default('http://localhost:8081').exec(),
      runtime: { minVersion: '0.0.1' },
    },
    campaign: {
      url: env('CAMPAIGN_SERVICE_URL').default('http://localhost:8084').exec(),
      runtime: { minVersion: '0.0.1' },
    },
    asset: {
      url: env('ASSET_SERVICE_URL').default('http://localhost:8086').exec(),
      dbVersioner: { minVersion: '1.41.4' },
      runtime: { minVersion: '1.41.4' },
    },
    notification: {
      url: env('NOTIFICATION_SERVICE_URL').default('http://localhost:8090').exec(),
    },
  },
  redis: {
    url: env('REDIS_URL').default('redis://localhost:6379/0').exec(),
    timeout: env('TIMEOUT_CACHE_ACCOUNT').default(300).exec(),
    password: env('REDIS_PASSWORD').default('').exec(),
    cachePrefix: env('REDIS_CACHE_PREFIX').default('search-service').exec(),
    syncTimeout: env('TIMEOUT_SYNCHRONIZATION').default(60).exec(),
  },
  cacheManager: env('CACHE_MANAGER').default('memory').exec(), // possible values: memory|redis
  serviceUid: env('SERVICE_UID').default('service_user').exec(),
  audit: {
    disableWatch: env('AUDIT_DISABLE_WATCH').default(true).bool().exec(),
  },
  cacheLifetime: env('CACHE_LIFETIME').default(5).exec(),
  restErrorFormat: {
    enabled: env('ERROR_FORMAT_ENABLED').default(true).bool().exec(),
    defaultSanitisedMessage: env('DEFAULT_SANITISED_ERROR_MESSAGE').default('An error occured contact support. Error ID:').exec(),
  },
  gqlErrorFormat: {
    enabled: env('GQL_ERROR_FORMAT_ENABLED').default(true).bool().exec(),
    defaultSanitisedMessage: env('GQL_DEFAULT_SANITISED_ERROR_MESSAGE').default('An error occured contact support. Error ID:').exec(),
  },
};

module.exports = config;

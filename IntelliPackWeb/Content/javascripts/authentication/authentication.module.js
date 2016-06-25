(function () {
  'use strict';

  angular
    .module('intellipackapp.authentication', [
      'intellipackapp.authentication.controllers',
      'intellipackapp.authentication.services'
    ]);

  angular
    .module('intellipackapp.authentication.controllers', []);

  angular
    .module('intellipackapp.authentication.services', ['ngCookies']);
})();
module app.shared {
    'use strict';

    class SideMenuController {

        static $inject = ['couponSummary', '$scope', '$ionicModal'];
        constructor(public couponSummary, private $scope, private $ionicModal) {
            this.$ionicModal.fromTemplateUrl('templates/Favorite.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.Favorite = modal; });
                this.$ionicModal.fromTemplateUrl('templates/Login.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.Login = modal; });
        }

        public ShowPopUp(): void {
            this.$scope.Favorite.show();
        }

        public ClosePopUp(): void {
            this.$scope.Favorite.hide();
        }
        public LoginShow(): void {
            this.$scope.Login.show();
        }

        public LoginHide(): void {
            this.$scope.Login.hide();
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.SideMenuController', SideMenuController);

}
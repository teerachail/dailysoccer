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
        }

        public ShowPopUp(): void {
            this.$scope.Favorite.show();
        }

        public ClosePopUp(): void {
            this.$scope.Favorite.hide();
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.SideMenuController', SideMenuController);

}
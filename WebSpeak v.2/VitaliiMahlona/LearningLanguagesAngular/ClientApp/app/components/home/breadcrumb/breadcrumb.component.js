var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
var BreadcrumbComponent = /** @class */ (function () {
    function BreadcrumbComponent(router, activeRoute) {
        this.router = router;
    }
    BreadcrumbComponent.prototype.ngOnInit = function () {
        this.setBreadcrumb();
    };
    BreadcrumbComponent.prototype.setBreadcrumb = function () {
        var _this = this;
        this.router.events.subscribe(function (val) {
            _this.displayBreadcrumbList = [];
            if (location.hash !== '#/') {
                _this.isHome = false;
                var paramIndex = location.hash.indexOf("?");
                if (paramIndex == -1) {
                    _this.route = location.hash;
                }
                else {
                    _this.route = location.hash.slice(0, paramIndex);
                }
                _this.masterBreadcrumbList = _this.route.split('/');
                _this.masterBreadcrumbList = _this.masterBreadcrumbList.slice(1, _this.masterBreadcrumbList.length);
                for (var i = 0; i < _this.masterBreadcrumbList.length; i++) {
                    if (i !== 0) {
                        _this.initialUrl = _this.displayBreadcrumbList[i - 1].url;
                    }
                    else {
                        _this.initialUrl = '/';
                    }
                    var breadCrumbObj = {
                        name: _this.masterBreadcrumbList[i],
                        url: _this.initialUrl + _this.masterBreadcrumbList[i] + '/',
                        id: i
                    };
                    _this.displayBreadcrumbList.push(breadCrumbObj);
                }
                _this.displayBreadcrumbList = _this.displayBreadcrumbList.slice(1);
                _this.lastLink = _this.displayBreadcrumbList[_this.displayBreadcrumbList.length - 1];
                _this.displayBreadcrumbList = _this.displayBreadcrumbList.slice(0, _this.displayBreadcrumbList.length - 1);
            }
            else {
                _this.isHome = true;
                var breadCrumbObj = {
                    name: 'Home',
                    url: '#',
                    id: -1
                };
                _this.lastLink = breadCrumbObj;
            }
        });
    };
    BreadcrumbComponent = __decorate([
        Component({
            selector: 'breadcrumb',
            templateUrl: './breadcrumb.component.html'
        }),
        __metadata("design:paramtypes", [Router, ActivatedRoute])
    ], BreadcrumbComponent);
    return BreadcrumbComponent;
}());
export { BreadcrumbComponent };
//# sourceMappingURL=breadcrumb.component.js.map
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
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../services/data.service';
var TestComponent = /** @class */ (function () {
    function TestComponent(dataService, activeRoute) {
        var _this = this;
        this.dataService = dataService;
        this.countOptions = 4;
        this.randomTestWordsId = [1, 2, 3, 4];
        this.totalResult = 0;
        this.first = true;
        this.questionNumber = 0;
        this.randomWords = [];
        this.checkboxes = [];
        this.textAnswer = '';
        this.notSelect = true;
        this.result = 0;
        this.numberQA = 0;
        this.randomWordsFor10 = [];
        this.questionNumberFor10 = 0;
        this.checkboxesLeft = [];
        this.checkboxesRight = [];
        this.randomWordsCopyLeft = [];
        this.randomWordsCopyRight = [];
        this.selectWordIdLeftFor10 = -1;
        this.selectWordIdRightFor10 = -1;
        this.randomWordsAnswerLeft = [];
        this.randomWordsAnswerRight = [];
        this.finishedTest = false;
        this.subscription = activeRoute.queryParams.subscribe(function (queryParam) {
            _this.idTest = queryParam['idTest'];
            if (_this.idTest == 1 || _this.idTest == 5) {
                _this.countOptions = 2;
                _this.randomTestWordsId = [1, 2];
            }
        });
    }
    TestComponent.prototype.ngOnInit = function () {
        this.loadWords();
    };
    TestComponent.prototype.loadWords = function () {
        var _this = this;
        this.dataService.getWords(this.idSubCat)
            .subscribe(function (data) {
            _this.firstId = data[0].id;
            _this.lastId = data[data.length - 1].id;
            _this.words = data.sort(_this.compareRandom);
            _this.randomWordsFor10 = _this.words;
            _this.totalQuestions = data.length;
            _this.idSubCat = data[0].subCategoryId;
            if (_this.idTest == 10) {
                _this.GetExtra();
                _this.totalQuestions = _this.randomWordsFor10.length;
                _this.check(false);
            }
            else {
                _this.check();
            }
        });
    };
    TestComponent.prototype.compareRandom = function (a, b) {
        return Math.random() - 0.5;
    };
    TestComponent.prototype.GetTestRandom = function () {
        var randomWordsId = [];
        if (this.words) {
            this.randomWords[0] = this.words[this.questionNumber - 1];
            randomWordsId[0] = this.words[this.questionNumber - 1].id;
        }
        var randomIds = [];
        for (var i = this.firstId; i <= this.lastId; ++i) {
            randomIds.push(i);
        }
        randomIds.splice(randomIds.indexOf(randomWordsId[0]), 1);
        randomIds.sort(this.compareRandom);
        for (var i = 1; i < this.countOptions; i++) {
            randomWordsId[i] = randomIds[0];
            randomIds.splice(0, 1);
            if (this.words) {
                for (var j = 0; j < this.words.length; j++) {
                    if (this.words[j].id == randomWordsId[i]) {
                        this.randomWords[i] = this.words[j];
                    }
                }
            }
        }
    };
    TestComponent.prototype.check = function (event) {
        var _this = this;
        if (event === void 0) { event = null; }
        if (this.idTest == 10) {
            this.randomWordsAnswerLeft = [];
            this.randomWordsAnswerRight = [];
            if (this.questionNumber == 0) {
                this.notSelect = false;
            }
            this.questionNumberFor10 = Math.floor((this.questionNumber + this.countOptions) / this.countOptions);
            if (event == false) {
                this.numberQA = 0;
                if (this.questionNumber == this.totalQuestions) {
                    this.finishedTest = true;
                    var DTOTest = {
                        totalResult: this.totalResult,
                        subCategoryId: this.idSubCat,
                        testNumber: this.idTest
                    };
                    this.dataService.setResultTest(DTOTest).subscribe(function (data) { return _this.isUser = data; });
                    return;
                }
                this.GetTest();
                this.randomTestWordsId.sort(this.compareRandom);
            }
            this.totalResult += this.result;
            this.result = 0;
        }
        if ((this.idTest == 6 || this.idTest == 7)) {
            this.notSelect = this.textAnswer.trim() == '';
            if (this.questionNumber == 0) {
                this.notSelect = false;
            }
            if (this.correctAnswer == this.textAnswer) {
                this.totalResult++;
                this.isCorrect = true;
            }
            else if (this.notSelect) {
                return;
            }
            else {
                this.isCorrect = false;
            }
            this.textAnswer = '';
        }
        if (this.idTest == 5) {
            if ((event != null) && (event.target.value == this.correctAnswer)) {
                this.totalResult++;
                this.isCorrect = true;
            }
            else {
                this.isCorrect = false;
            }
        }
        if (this.questionNumber == 1) {
            this.first = false;
        }
        if (this.idTest != 5 && this.idTest != 6 && this.idTest != 7 && this.idTest != 10) {
            this.notSelect = this.checkboxes.length == 0;
            if (this.questionNumber == 0) {
                this.notSelect = false;
            }
            if (this.checkboxes.indexOf(true) + 1 == this.correctAnswer) {
                this.isCorrect = true;
                this.totalResult++;
            }
            else if (this.notSelect) {
                if (!this.first) {
                    this.checkboxes = [];
                    return;
                }
            }
            else {
                this.isCorrect = false;
            }
            this.checkboxes = [];
        }
        if (this.questionNumber == this.totalQuestions) {
            this.finishedTest = true;
            var DTOTest = {
                totalResult: this.totalResult,
                subCategoryId: this.idSubCat,
                testNumber: this.idTest
            };
            this.dataService.setResultTest(DTOTest).subscribe(function (data) { return _this.isUser = data; });
            return;
        }
        if (this.idTest != 10) {
            this.questionNumber++;
        }
        if (this.idTest == 6 || this.idTest == 7) {
            this.GetTestFor6_7();
            this.correctAnswer = this.randomWord.wordLearnLang;
        }
        else if (this.idTest != 10) {
            this.randomTestWordsId.sort(this.compareRandom);
            this.GetTestRandom();
            for (var i = 0; i < Object.keys(this.randomWords).length; i++) {
                if (this.randomTestWordsId[i] - 1 == 0) {
                    this.correctAnswer = i + 1;
                }
            }
        }
    };
    TestComponent.prototype.changeAnswer = function (eventTarget) {
        this.checkboxes = [];
        this.checkboxes[eventTarget.getAttribute('value')] = eventTarget.checked;
    };
    TestComponent.prototype.changeAnswerLeft = function (eventTarget) {
        this.checkboxesLeft = [];
        this.checkboxesLeft[eventTarget.getAttribute('value')] = eventTarget.checked;
        this.selectWordIdLeftFor10 = +eventTarget.getAttribute('id').substring(10, 11);
    };
    TestComponent.prototype.changeAnswerRight = function (eventTarget) {
        this.checkboxesRight = [];
        this.checkboxesRight[eventTarget.getAttribute('value')] = eventTarget.checked;
        this.selectWordIdRightFor10 = +eventTarget.getAttribute('id').substring(11, 12);
    };
    TestComponent.prototype.GetTestFor6_7 = function () {
        this.randomWord = this.words[this.questionNumber - 1];
    };
    TestComponent.prototype.GetExtra = function () {
        var remainderOfDivision = this.randomWordsFor10.length % this.countOptions;
        if (remainderOfDivision == 0) {
            return;
        }
        var randomIds = [];
        for (var i = this.firstId; i <= this.lastId; ++i) {
            randomIds.push(i);
        }
        for (var i = 0; i < remainderOfDivision; ++i) {
            randomIds.splice(randomIds.indexOf(this.randomWordsFor10[this.randomWordsFor10.length - i - 1].id), 1);
        }
        randomIds.sort(this.compareRandom);
        for (var i = 0; i < 4 - remainderOfDivision; ++i) {
            this.randomWordsFor10.push(this.words[i]);
        }
    };
    TestComponent.prototype.cancel = function () {
        this.notSelect = false;
        this.questionNumber -= this.result;
        this.result = 0;
        this.numberQA = 0;
        this.check(true);
        this.randomWordsCopyLeft = Object.assign([], this.randomWords);
        this.randomWordsCopyRight = Object.assign([], this.randomWords);
        this.randomWordsAnswerLeft = [];
        this.randomWordsAnswerRight = [];
    };
    TestComponent.prototype.GetTest = function () {
        for (var i = 0; i < this.countOptions; i++) {
            this.randomWords[i] = this.randomWordsFor10[0];
            this.randomWordsCopyLeft[i] = this.randomWordsFor10[0];
            this.randomWordsCopyRight[i] = this.randomWordsFor10[0];
            this.randomWordsFor10.splice(0, 1);
        }
    };
    TestComponent.prototype.next = function () {
        this.notSelect = false;
        var isCorrectAnswer = false;
        if ((this.checkboxesLeft.indexOf(true) == this.checkboxesRight.indexOf(true)) && (this.checkboxesLeft.indexOf(true) != -1)) {
            isCorrectAnswer = true;
        }
        if (this.checkboxesLeft.indexOf(true) != -1 && this.checkboxesRight.indexOf(true) != -1) {
            this.questionNumber++;
            this.checkboxesLeft = [];
            this.checkboxesRight = [];
            this.numberQA++;
            this.randomWordsAnswerLeft.push(this.randomWordsCopyLeft[this.selectWordIdLeftFor10]);
            this.randomWordsAnswerRight.push(this.randomWordsCopyRight[this.selectWordIdRightFor10]);
            delete this.randomWordsCopyLeft[this.selectWordIdLeftFor10];
            delete this.randomWordsCopyRight[this.selectWordIdRightFor10];
            if (isCorrectAnswer) {
                this.questionNumberFor10 = Math.floor((this.questionNumber + this.countOptions - 1) / this.countOptions);
                this.result++;
            }
        }
        else {
            this.notSelect = true;
            return;
        }
    };
    TestComponent.prototype.again = function () {
        window.location.reload();
    };
    TestComponent = __decorate([
        Component({
            selector: 'test',
            templateUrl: './test.component.html',
            styleUrls: ['./test.component.scss']
        }),
        __metadata("design:paramtypes", [DataService, ActivatedRoute])
    ], TestComponent);
    return TestComponent;
}());
export { TestComponent };
//# sourceMappingURL=test.component.js.map
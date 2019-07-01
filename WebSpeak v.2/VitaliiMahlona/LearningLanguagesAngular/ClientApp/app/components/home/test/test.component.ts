import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from '../../../services/data.service';
import { DTO } from '../../../models/DTO';

@Component({
    selector: 'test',
    templateUrl: './test.component.html',
    styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {
    idSubCat: number;
    idTest: number;
    words: DTO[];
    correctAnswer: any;
    countOptions: number = 4;
    randomTestWordsId: number[] = [1, 2, 3, 4];
    totalResult: number = 0;
    first: boolean = true;
    questionNumber: number = 0;
    totalQuestions: number;
    firstId: number;
    lastId: number;
    randomWords: DTO[] = [];
    randomWord: DTO;
    checkboxes: boolean[] = [];
    textAnswer: string = '';
    isCorrect: boolean;
    notSelect: boolean = true;
    result: number = 0;
    numberQA: number = 0;
    randomWordsFor10: DTO[] = [];
    questionNumberFor10: number = 0;
    checkboxesLeft: boolean[] = [];
    checkboxesRight: boolean[] = [];
    randomWordsCopyLeft: DTO[] = [];
    randomWordsCopyRight: DTO[] = [];
    selectWordIdLeftFor10: number = -1;
    selectWordIdRightFor10: number = -1;
    randomWordsAnswerLeft: DTO[] = [];
    randomWordsAnswerRight: DTO[] = [];
    finishedTest: boolean = false;
    isUser: boolean;

    private subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
        this.subscription = activeRoute.queryParams.subscribe(
            (queryParam: any) => {
                this.idTest = queryParam['idTest'];

                if (this.idTest == 1 || this.idTest == 5) {
                    this.countOptions = 2;
                    this.randomTestWordsId = [1, 2];
                }
            }
        );
    }

    ngOnInit() {
        this.loadWords();
    }

    loadWords() {
        this.dataService.getWords(this.idSubCat)
            .subscribe((data: DTO[]) => {
                this.firstId = data[0].id;
                this.lastId = data[data.length - 1].id;
                this.words = data.sort(this.compareRandom);
                this.randomWordsFor10 = this.words;
                this.totalQuestions = data.length;
                this.idSubCat = data[0].subCategoryId;

                if (this.idTest == 10) {
                    this.GetExtra();
                    this.totalQuestions = this.randomWordsFor10.length;
                    this.check(false);
                }
                else {
                    this.check();
                }
            });
    }

    compareRandom(a:any, b:any) {
        return Math.random() - 0.5;
    }

    GetTestRandom() {
        var randomWordsId = [];

        if (this.words) {
            this.randomWords[0] = this.words[this.questionNumber - 1];
            randomWordsId[0] = this.words[this.questionNumber - 1].id;
        }

        var randomIds = [];

        for (let i = this.firstId; i <= this.lastId; ++i) {
            randomIds.push(i);
        }

        randomIds.splice(randomIds.indexOf(randomWordsId[0]), 1);

        randomIds.sort(this.compareRandom);

        for (let i = 1; i < this.countOptions; i++) {
            randomWordsId[i] = randomIds[0];

            randomIds.splice(0, 1);

            if (this.words) {
                for (let j = 0; j < this.words.length; j++) {
                    if (this.words[j].id == randomWordsId[i]) {
                        this.randomWords[i] = this.words[j];
                    }
                }
            }
        }
    }

    check(event: any = null) {
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

                    this.dataService.setResultTest(DTOTest).subscribe((data: boolean) => this.isUser = data);

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

            this.dataService.setResultTest(DTOTest).subscribe((data: boolean) => this.isUser = data);

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

            for (let i = 0; i < Object.keys(this.randomWords).length; i++) {
                if (this.randomTestWordsId[i] - 1 == 0) {
                    this.correctAnswer = i + 1;
                }
            }
        }
    }

    changeAnswer(eventTarget: any) {
        this.checkboxes = [];
        this.checkboxes[eventTarget.getAttribute('value')] = eventTarget.checked;
    }

    changeAnswerLeft(eventTarget: any) {
        this.checkboxesLeft = [];
        this.checkboxesLeft[eventTarget.getAttribute('value')] = eventTarget.checked;
        this.selectWordIdLeftFor10 = +eventTarget.getAttribute('id').substring(10, 11);
    }

    changeAnswerRight(eventTarget: any) {
        this.checkboxesRight = [];
        this.checkboxesRight[eventTarget.getAttribute('value')] = eventTarget.checked;
        this.selectWordIdRightFor10 = +eventTarget.getAttribute('id').substring(11, 12);
    }

    GetTestFor6_7() {
        this.randomWord = this.words[this.questionNumber - 1];
    }

    GetExtra() {
        var remainderOfDivision = this.randomWordsFor10.length % this.countOptions;

        if (remainderOfDivision == 0) {
            return;
        }

        var randomIds = [];

        for (let i = this.firstId; i <= this.lastId; ++i) {
            randomIds.push(i);
        }

        for (let i = 0; i < remainderOfDivision; ++i) {
            randomIds.splice(randomIds.indexOf(this.randomWordsFor10[this.randomWordsFor10.length - i - 1].id), 1);
        }

        randomIds.sort(this.compareRandom);

        for (let i = 0; i < 4 - remainderOfDivision; ++i) {
            this.randomWordsFor10.push(this.words[i]);
        }
    }

    cancel() {
        this.notSelect = false;
        this.questionNumber -= this.result;
        this.result = 0;
        this.numberQA = 0;
        this.check(true);
        this.randomWordsCopyLeft = Object.assign([], this.randomWords);
        this.randomWordsCopyRight = Object.assign([], this.randomWords);
        this.randomWordsAnswerLeft = [];
        this.randomWordsAnswerRight = [];
    }

    GetTest() {
        for (let i = 0; i < this.countOptions; i++) {
            this.randomWords[i] = this.randomWordsFor10[0];
            this.randomWordsCopyLeft[i] = this.randomWordsFor10[0];
            this.randomWordsCopyRight[i] = this.randomWordsFor10[0];

            this.randomWordsFor10.splice(0, 1);
        }
    }

    next() {
        this.notSelect = false;
        var isCorrectAnswer: boolean = false;

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
    }

    again() {
        window.location.reload();
    }
}
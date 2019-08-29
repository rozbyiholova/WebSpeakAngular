import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Constants } from './Constants';
import { CheckMethod } from './CheckMethod';
import { TestResult } from './TestResult';
import { FillMethod } from './FillMethod';

let data = require("./testssettings.json");

export class TestInfo {

    private http: HttpClient;
    private testId: number;
    private _currentIndex: number;
    private _categories: any[];
    private _testSetting: Object;
    public checkMethod: Function = new Function();
    public testResult: TestResult;
    
    constructor(array: any[], testId: number) {
        this._currentIndex = 0;
        this._categories = array;
        this.testId = testId;
        this.testResult = new TestResult();
        const settingString = JSON.stringify(data);
        const settingObject = JSON.parse(settingString);
        this._testSetting = settingObject[`Test${this.testId}`];
        
        this.testResult.correct.subscribe((word: string) => this.onCorrectAnswer(word));
        this.testResult.incorrect.subscribe((word: string) => this.onIncorrectAnswer(word));

        this.onInit();
    }
    
    private onInit(): void {
        switch (+this.testId) {
        case 1:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 2:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 3:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 4:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 5:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 6:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 7:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 8:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 9:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        case 10:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            break;
        }
        }

        this.setConfirmButton(this.testId);
    }

    public increaseIndex(): void {
        this._currentIndex++;
    }
    public get currentIndex(): number {
        return this._currentIndex;
    }

    public get setting(): Object {
        return this._testSetting;
    }

    public get categories(): any[] {
        return this._categories;
    }

    private setConfirmButton(testId: number): void {
        let button: HTMLElement = document.querySelector('.confirm') as HTMLElement;
        let buttonYes: HTMLElement, buttonNo: HTMLElement;
        if (button != null && testId != 5) {
            button.addEventListener('click', (e: Event) => {
                this.checkMethod(this.testResult);
            });
        } else {
            buttonYes = document.querySelector('.button_yes') as HTMLElement;
            buttonNo = document.querySelector('.button_no') as HTMLElement;
            buttonYes.addEventListener('click', (e: Event) => this.checkMethod());
            buttonNo.addEventListener('click', (e: Event) => this.checkMethod());
        }
    }

    public loadNextTest(): void {
        if (this.currentIndex < this.categories.length) {
            FillMethod.LoadPictures(this);
            FillMethod.LoadText(this);
            this.increaseIndex();
        }
    }

    private onCorrectAnswer(word: string): void {
        console.log("caught correct answer");
        this.testResult.questionNames.push(word);
        this.testResult.questionResults.push("correct");
        this.loadNextTest();
    }

    private onIncorrectAnswer(word: string): void {
        console.log("caught incorrect answer");
        this.testResult.questionNames.push(word);
        this.testResult.questionResults.push("incorrect");
        this.loadNextTest();
    }



    /*
     TODO:
     - deal with styles
     - write "GenerateResult" method
     - create method for loading tests e.g. "LoadTest5", "LoadTest10" and use them in "loadNewTest" method
        depending on current testId;
     */
}
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from "../../auth.service";
import { DataService } from "../../data.service";

import { Constants } from './Constants';
import { CheckMethod } from './CheckMethod';
import { TestResult } from './TestResult';
import { FillMethod } from './FillMethod';

import { User } from "../../../Models/User";

let data = require("./testssettings.json");

export class TestInfo {

    private readonly testId: number;
    private readonly categoryId: number;
    private readonly _categories: any[];
    private readonly _testSetting: Object;
    private readonly user: User;
    private _currentIndex: number;
    public testResult: TestResult;
    public checkMethod: Function = new Function();
    public fillMethods: Function[] = new Array<Function>();

    private readonly picturesWordsSounds: Function[] = [
        FillMethod.loadPictures,
        FillMethod.loadText,
        FillMethod.loadSounds
    ];
    
    constructor(
        array: any[],
        testId: number,
        categoryId: number,
        user?: User) {
        this._currentIndex = 0;
        this._categories = array;
        this.testId = testId;
        this.categoryId = categoryId;
        this.user = user;
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
            this.fillMethods = this.picturesWordsSounds;
            break;
        }
        case 2:
        {
            this.checkMethod = CheckMethod.checkPictureWithText;
            this.fillMethods = this.picturesWordsSounds;
            break;
        }
        case 3:
        {
            this.checkMethod = CheckMethod.checkTextWithPicture;
            this.fillMethods = this.picturesWordsSounds;
            break;
        }
        case 4:
        {
            this.checkMethod = CheckMethod.checkPictureWithSound;
            this.fillMethods = this.picturesWordsSounds;
            break;
        }
        case 5:
        {
            this.checkMethod = CheckMethod.checkTrueOrFalse;
            this.fillMethods = [
                FillMethod.loadPictures,
                FillMethod.loadRandomText
            ];
            break;
        }
        case 6:
        {
                    this.checkMethod = CheckMethod.checkPictureWithInput;
                    this.fillMethods = [
                        FillMethod.loadPictures,
                        FillMethod.enableInput
                    ];
            break;
        }
        case 7:
        {
                    this.checkMethod = CheckMethod.checkSoundWithInput;
                    this.fillMethods = [
                        FillMethod.loadSounds,
                        FillMethod.enableInput
                    ];
            break;
        }
        case 8:
        {
                    this.checkMethod = CheckMethod.checkTranslationWithNative;
                    this.fillMethods = [
                        FillMethod.loadNativeText
                    ];
            break;
        }
        case 9:
        {
                    this.checkMethod = CheckMethod.checkSoundWithText;
                    this.fillMethods = [
                        FillMethod.loadSounds,
                        FillMethod.loadText
                    ];
            break;
        }
        case 10:
        {
            this.checkMethod = this.checkTenth;
                    this.fillMethods = [
                        FillMethod.loadPairs
                    ];
            break;
        }
        }

        this.setConfirmButton(this.testId);
    }

    public increaseIndex(): void {
        this._currentIndex++;
    }
    public reduceIndex(): void {
        this._currentIndex--;
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

    public get result(): TestResult {
        return this.testResult;
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
            buttonYes.addEventListener('click', (e: Event) => this.checkMethod(this.testResult));
            buttonNo.addEventListener('click', (e: Event) => this.checkMethod(this.testResult));
        }
    }

    public loadNextTest(): void {
        if (this.currentIndex < this.categories.length) {
            this.runFillMethods();
            this.increaseIndex();
        } else {
            this.generateResult(this.testResult);
        }
    }

    private onCorrectAnswer(word: string): void {
        this.testResult.questionNames.push(word);
        this.testResult.questionResults.push("correct");
        this.loadNextTest();
    }

    private onIncorrectAnswer(word: string): void {
        this.testResult.questionNames.push(word);
        this.testResult.questionResults.push("incorrect");
        this.loadNextTest();
    }

    private generateResult(testResult: TestResult): void {
        if (testResult) {

            const resultForSaving = this.generateTestResultForSaving(testResult);

            if (resultForSaving) {
                this.testResult.emitTestEnded(resultForSaving);
            }

            let testResultDiv = document.getElementsByClassName(Constants.TEST_RESULT)[0] as HTMLElement;
            let testDiv = document.getElementsByClassName(Constants.TEST)[0] as HTMLElement;
            let names = testResult.questionNames;
            let testResults = testResult.questionResults;

            if (!testDiv) {
                testDiv = document.querySelector(".test10") as HTMLElement;
                document.querySelector(".intermediate").innerHTML = "";
            }

            testDiv.setAttribute("style", "display: none");
            testResultDiv.setAttribute("style", "display: block;");
            let table: HTMLTableElement = document.createElement("table");

            for (let i = 0; i < testResult.getLength(); i++) {
                let tr: HTMLTableRowElement = document.createElement("tr");
                const nameTextNode: Text = document.createTextNode(names[i]);
                const resultTextNode: Text = document.createTextNode(testResults[i]);

                let tdName: HTMLTableDataCellElement = document.createElement("td");
                let tdResult: HTMLTableDataCellElement = document.createElement("td");
                tdName.appendChild(nameTextNode);
                tdResult.appendChild(resultTextNode);

                tr.appendChild(tdName);
                tr.appendChild(tdResult);
                table.appendChild(tr);
            }

            let trTotal = document.createElement("tr");
            let total = testResult.getTotal();
            trTotal.innerHTML = "<td>Total</td>";
            trTotal.innerHTML += `<td>${total}</td>`;
            table.appendChild(trTotal);

            testResultDiv.appendChild(table);
        } else {
            return;
        }
    }

    private runFillMethods(): void {
        this.fillMethods.forEach(method => method(this));
    }

    private checkTenth(): void {
        const btnCount: number = document.getElementsByClassName("btn").length;
        const disabledCount: number = document.querySelectorAll(":disabled").length;
        if (btnCount === disabledCount) {
            this.loadNextTest();
        } else {
            alert("Make all the pairs, please");
        }
    }

    private generateTestResultForSaving(result: TestResult): Object | null {
        if (this.user) {
            const totalResult: number = result.getTotal();
            return {
                TestId: this.testId,
                Result: totalResult,
                UserId: this.user.Id,
                LangId: this.user.UserSettings["LearningLanguageId"],
                CategoryId: this.categoryId,
                TestDate: Date.now()
            }
        }

        return null;
    }
}
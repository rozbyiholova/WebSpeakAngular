import { Output, EventEmitter } from '@angular/core';
export class TestResult {

    constructor() {
        this.questionNames = new Array();
        this.questionResults = new Array();
    }

    @Output() correct: EventEmitter<string> = new EventEmitter();
    @Output() incorrect: EventEmitter<string> = new EventEmitter();
    @Output() testEnded: EventEmitter<Object> = new EventEmitter();

    questionNames: string[];
    questionResults: string[];
    
    public getLength(): number {
        return Math.min(this.questionNames.length, this.questionNames.length);
    }
    public getTotal(): number {
        let sum = 0;
        for (let i = 0; i < this.getLength(); i++) {
            if (this.questionResults[i] === "correct") { sum++; }
        }
        return sum;
    }
    
    public emitCorrectAnswer(word: string): void {
        this.correct.emit(word);
    }
    public emitIncorrectAnswer(word: string): void {
        this.incorrect.emit(word);
    }
    public emitTestEnded(result: Object): void {
        console.log("TestResult - emitTestEnded");
        this.testEnded.emit(result);
    }
}
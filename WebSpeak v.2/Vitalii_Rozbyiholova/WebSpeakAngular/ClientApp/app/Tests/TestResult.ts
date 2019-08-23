export class TestResult {
    questionNames: string[];
    questionResults: string[];
    
    getLength() {
        return this.questionNames.length;
    }

    getTotal() {
        let sum = 0;
        for (let i = 0; i < this.getLength(); i++) {
            if (this.questionResults[i] === "correct") { sum++; }
        }
        return sum;
    }

    correctAnswear = (word) => {
        info.increaseScore();
        this.questionNames.push(word);
        this.questionResults.push("correct");
        NextTest();
    }

    uncorrectAnswear = (word) => {
        this.questionNames.push(word);
        this.questionResults.push("uncorrect");
        NextTest();
    }
}
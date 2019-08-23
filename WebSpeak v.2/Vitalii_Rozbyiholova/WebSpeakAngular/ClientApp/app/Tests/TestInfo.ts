import { Constants } from './Constants' 

export default class TestInfo {

    private currentIndex: number;
    private categories: any[];
    private testId: number;
    private currentScore: number;
    private testSetting: object;
    public checkMethod: Function;
    
    constructor(array: any[], testId: number) {
        this.currentIndex = 0;
        this.currentScore = 0;
        this.categories = array;
        this.testId = testId;


        switch (this.testId) {
            case 1:
            {
                    this.checkMethod = this.checkPictureWithText;
                    break;
                }
            case 2:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
            }
            case 3:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
                }
            case 4:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
                }
            case 5:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
                }
            case 6:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
                }
            case 7:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
                }
            case 8:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
                }
            case 9:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
                }
            case 10:
            {
                this.checkMethod = this.checkPictureWithText;
                break;
            }
        }

        this.setConfirmButton();
    }

    increaseIndex() {
        this.currentIndex++;
    }

    increaseScore() {
        this.currentScore++;
    }

    getCurrentScore(): number {
        return this.currentScore;
    }

    private readJson(err: any, data: any): void {
        if (err) { throw err; }
        this.testSetting = JSON.parse(data);
    }

    private setConfirmButton(): void {
        let button: HTMLElement = document.querySelector('.confirm') as HTMLElement;
        let button_yes: HTMLElement, button_no: HTMLElement;
        if (button != null) {
            button.addEventListener('click', (e: Event) => this.checkMethod);
        } else {
            button_yes = document.querySelector('.button_yes') as HTMLElement;
            button_no = document.querySelector('.button_no') as HTMLElement;
            button_yes.addEventListener('click', (e: Event) => this.checkMethod);
            button_no.addEventListener('click', (e: Event) => this.checkMethod);
        }
    }

    private checkPictureWithText(): void {
        
        let test_word_div = document.querySelector('.' + Constants.TEST_WORD) as HTMLElement;
        let label = test_word_div.getElementsByTagName('label')[0] as HTMLElement;
        let word = label.childNodes[0].textContent;
        let selectedLabel = document.querySelector('.selected_picture') as HTMLElement;

        if (selectedLabel != undefined && word != undefined) {
            let picture = selectedLabel.childNodes[0] as HTMLPictureElement;
            if (picture.dataset["answer"] == word) {
                selectedLabel.classList.remove('selected_picture');
                testResult.correctAnswear(word);
            } else {
                selectedLabel.classList.remove('selected_picture');
                testResult.uncorrectAnswear(word);
            }
        } else {
            alert("Nothing selected. Please select any item");
        }
    }


    /*
     TODO:
     - create class for test initialization (pictures, words, sounds etc)
     - create class for checking methods
     - set answer inside data-attribute 
     */
}
import { Constants } from './Constants';
import { TestResult } from './TestResult';

export class CheckMethod {
    public static checkPictureWithText(result: TestResult): void {
        let test_word_div = document.querySelector('.' + Constants.TEST_WORD) as HTMLElement;
        let label = test_word_div.getElementsByTagName('label')[0] as HTMLElement;
        let word = label.childNodes[0].textContent;
        let selectedPicture = document.querySelector('.selected_picture') as HTMLElement;

        if (selectedPicture != undefined && word != undefined) {
            if (selectedPicture.dataset["answer"] == word) {
                selectedPicture.classList.remove('selected_picture');
                result.emitCorrectAnswer(word);
            } else {
                selectedPicture.classList.remove('selected_picture');
                result.emitIncorrectAnswer(word);
            }
        } else {
            alert("Nothing selected. Please select any item");
        }
    }

    public static checkTrueOrFalse(result: TestResult): void {
        const buttonYes = document.querySelector(`.${Constants.BUTTON_YES}`) as HTMLElement;
        const buttonNo = document.querySelector(`.${Constants.BUTTON_NO}`) as HTMLElement;

        const pictureDiv = document.querySelector(`.${Constants.TEST_IMAGES}`) as HTMLElement;
        const picture = pictureDiv.getElementsByTagName('label')[0].childNodes[0] as HTMLElement;
        let text = picture.dataset["answer"];
        let textDiv = document.getElementsByClassName(Constants.TEST_WORD)[0] as HTMLElement;
        let word = textDiv.childNodes[0].textContent;

        if (word && picture) {

            const checkText = text == word;
            const yesPressed = buttonYes.classList.contains(Constants.CLICKED);
            const noPressed = buttonNo.classList.contains(Constants.CLICKED);

            if ((checkText && yesPressed) || (!checkText && noPressed)) {
                result.emitCorrectAnswer(text);
            } else {
                result.emitIncorrectAnswer(text);
            }
        }
    }

    private static isReady(fn): void {
        if (document.readyState != 'loading') {
            fn();
        } else {
            document.addEventListener('DOMContentLoaded', fn);
        }
    }
}
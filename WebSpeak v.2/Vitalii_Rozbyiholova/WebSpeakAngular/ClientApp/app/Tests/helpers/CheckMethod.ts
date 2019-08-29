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
    
}
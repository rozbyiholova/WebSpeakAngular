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
            alert(Constants.ALERT_MESSAGE);
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

    public static checkPictureWithSound(result: TestResult): void {
        const testSoundDiv = document.getElementsByClassName(Constants.TEST_SOUNDS)[0] as HTMLElement;
        const sound = testSoundDiv.childNodes[0] as HTMLElement;
        const answer: string = sound.dataset["answer"];
        const selectedPicture = document.querySelector(`.${Constants.SELECTED_PICTURE}`) as HTMLPictureElement;

        if (selectedPicture && answer) {
            const pictureAnswer: string = selectedPicture.dataset["answer"];

            if (pictureAnswer === answer) {
                selectedPicture.classList.remove(`.${Constants.SELECTED_PICTURE}`);
                result.emitCorrectAnswer(answer);
            } else {
                selectedPicture.classList.remove(`.${Constants.SELECTED_PICTURE}`);
                result.emitIncorrectAnswer(answer);
            }
        } else {
            alert(Constants.ALERT_MESSAGE);
        }
    }

    public static checkTextWithPicture(result: TestResult): void {

        const testPictureDiv = document.getElementsByClassName(Constants.TEST_IMAGES)[0] as HTMLElement;
        const label = testPictureDiv.getElementsByTagName('label')[0] as HTMLElement;
        const picture = label.childNodes[0] as HTMLPictureElement;
        const pictureText: string = picture.dataset["answer"];
        const selectedTextElement = document.querySelector(`.${Constants.SELECTED_TEXT}`) as HTMLElement;

        if (picture && selectedTextElement) {
            const selectedText: string = selectedTextElement.textContent;

            if (pictureText === selectedText) {
                selectedTextElement.classList.remove(Constants.SELECTED_PICTURE);
                result.emitCorrectAnswer(selectedText);
            } else {
                selectedTextElement.classList.remove(Constants.SELECTED_PICTURE);
                result.emitIncorrectAnswer(selectedText);
            }
        } else {
            alert(Constants.ALERT_MESSAGE);
        }
    }

    public static checkPictureWithInput(result: TestResult): void {
        const testInputDiv = document.querySelector(`.${Constants.TEST_INPUT}`) as HTMLElement;
        let input = testInputDiv.getElementsByTagName("input")[0];
        const value: string = input.value;
        const testPictureDiv = document.querySelector(`.${Constants.TEST_IMAGES}`) as HTMLElement;
        const label = testPictureDiv.getElementsByTagName("label")[0];
        const picture = label.childNodes[0] as HTMLElement;
        const answer: string = picture.dataset["answer"];

        if (value && answer) {

            if (value === answer) {
                picture.classList.remove(Constants.SELECTED_PICTURE);
                result.emitCorrectAnswer(answer);
            } else {
                picture.classList.remove(Constants.SELECTED_PICTURE);
                result.emitIncorrectAnswer(answer);
            }

            input.value = "";
        } else {
            alert(Constants.ALERT_MESSAGE);
        }
    }

    public static checkSoundWithInput(result: TestResult): void {
        const testInputDiv = document.querySelector(`.${Constants.TEST_INPUT}`) as HTMLElement;
        const input = testInputDiv.getElementsByTagName("input")[0];
        let value = input.value;
        const testSoundDiv = document.querySelector(`.${Constants.TEST_SOUNDS}`) as HTMLElement;
        const sound = testSoundDiv.childNodes[0] as HTMLElement;
        const word: string = sound.dataset["answer"];

        if (input && word) {

            if (value === word) {
                result.emitCorrectAnswer(word);
            } else {
                result.emitIncorrectAnswer(word);
            }

            input.value = "";
        } else {
            alert(Constants.ALERT_MESSAGE);
        }
    }

    public static checkSoundWithText(result: TestResult): void {
        const testSoundDiv = document.querySelector(`.${Constants.TEST_SOUNDS}`) as HTMLElement;
        const sound = testSoundDiv.childNodes[0] as HTMLElement;
        const answer = sound.dataset["answer"];

        const selectedTextHeading = document.querySelector(`.${Constants.SELECTED_TEXT}`) as HTMLElement;

        if (selectedTextHeading && answer) {
            const selectedText: string = selectedTextHeading.innerText;

            if (selectedText === answer) {
                result.emitCorrectAnswer(answer);
            } else {
                result.emitIncorrectAnswer(answer);
            }

            selectedTextHeading.classList.remove(Constants.SELECTED_TEXT);
        } else {
            alert(Constants.ALERT_MESSAGE);
        }
    }

    public static checkTranslationWithNative(result: TestResult): void {
        const translationWordDiv = document.querySelector(`.${Constants.TEST_FOREIGN}`) as HTMLElement;
        const translationHeading = translationWordDiv.childNodes[0] as HTMLElement;
        const answer: string = translationHeading.textContent;
        const selectedText = document.querySelector(`.${Constants.SELECTED_TEXT}`) as HTMLElement;

        if (selectedText && answer) {
            let nativeWord = selectedText.dataset["answer"];

            if (nativeWord === answer) {
                result.emitCorrectAnswer(answer);
            } else {
                result.emitIncorrectAnswer(answer);
            }

            selectedText.classList.remove(Constants.SELECTED_PICTURE);
        } else {
            alert(Constants.ALERT_MESSAGE);
        }
    }

    public static checkPair(result: TestResult): void {
        const nativeButton = document.querySelector(`.${Constants.NATIVE} .btn-warning`) as HTMLElement;
        const foreignButton = document.querySelector(`.${Constants.FOREIGN} .btn-warning`) as HTMLElement;

        if (nativeButton && foreignButton) {
            const text: string = foreignButton.textContent;
            const answer: string = nativeButton.dataset["answer"];

            //using direct pushing without emitting event
            //to prevent loading new test
            //(TestInfo class catches event and starts loading new test)
            result.questionNames.push(answer);

            if (text === answer) {
                result.questionResults.push("correct");
                nativeButton.classList.replace("btn-warning", "btn-success");
                foreignButton.classList.replace("btn-warning", "btn-success");
            } else {
                result.questionResults.push("incorrect");
                nativeButton.classList.replace("btn-warning", "btn-danger");
                foreignButton.classList.replace("btn-warning", "btn-danger");
            }

            nativeButton.setAttribute("disabled", "true");
            foreignButton.setAttribute("disabled", "true");
        }
    }
}
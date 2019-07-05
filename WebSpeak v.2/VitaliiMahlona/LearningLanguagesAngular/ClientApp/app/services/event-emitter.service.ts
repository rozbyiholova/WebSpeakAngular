import { Injectable, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';

@Injectable({
    providedIn: 'root'
})
export class EventEmitterService {

    invokeUsersInfo = new EventEmitter();
    subsVar: Subscription;

    constructor() { }

    onAnotherComponentUpdateUsersInfo() {
        this.invokeUsersInfo.emit();
    }
}  
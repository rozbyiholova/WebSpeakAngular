import { Component } from '@angular/core';

import { DataService } from '../services/data.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    providers: [DataService]
})
export class AppComponent { }
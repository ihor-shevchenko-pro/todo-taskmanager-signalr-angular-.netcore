import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-todotask',
  templateUrl: './todotask.component.html',
  styleUrls: ['./todotask.component.css']
})
export class TodotaskComponent implements OnInit {

  public model: NgbDateStruct;

  constructor(private _formBuilder: FormBuilder,) { }

  ngOnInit(): void {
  }

  // mainForm validator
  public addToDoTaskModel = this._formBuilder.group({
    Title: [null, [Validators.required]],
    Description: [null, [Validators.required]],
    ExpirationDate: [null],
    ToUserId: [null]
  });

  public createToDoTask(): void {
    console.log("Boom");
  }

}

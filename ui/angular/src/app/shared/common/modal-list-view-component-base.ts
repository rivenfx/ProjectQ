import { Injector, Input, Directive, Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SFComponent } from '@delon/form';
import { ListViewComponentBase } from '@shared/common/list-view-component-base';
import { NzModalRef } from 'ng-zorro-antd/modal';

@Component({
  template: '',
})
// tslint:disable-next-line:component-class-suffix
export abstract class ModalListViewComponentBase<TModal, TList> extends ListViewComponentBase<TList> {
  private _modalInput: TModal;
  private _readonly: boolean;

  /** 标题前缀 */
  titlePrefix = this.l('label.create');
  /** 标题 */
  title = '';
  /** 模态框指针 */
  modalRef: NzModalRef;
  /** 编辑 */
  isEdit: boolean;

  /** 外部输入参数 */
  @Input()
  set modalInput(val: TModal) {
    this._modalInput = val;
    if (typeof val !== 'undefined') {
      this.isEdit = true;
    }
    if (!!val && !this.readonly) {
      this.titlePrefix = this.l('label.edit');
    }
  }

  /** 外部输入参数 */
  get modalInput(): TModal {
    return this._modalInput;
  }

  /** 只读 */
  set readonly(val: boolean) {
    this._readonly = val;
    if (val) {
      this.titlePrefix = this.l('label.readonly');
    }
  }

  /** 只读 */
  get readonly(): boolean {
    return this._readonly;
  }

  /** 页面表单 */
  @ViewChild('pageForm', { static: false }) pageFormRef: SFComponent;

  constructor(injector: Injector) {
    super(injector);
    try {
      this.modalRef = injector.get(NzModalRef);
    } catch (e) { }
  }

  /** 关闭模态框-成功 */
  success(res: boolean | any = true) {
    if (this.modalRef) {
      this.modalRef.close(res);
    }
  }

  /** 关闭模态框-直接关闭 */
  close(res: boolean | any = false) {
    this.success(res);
  }

  /** 当页面状态为只读, 修改表单控件状态为禁用 */
  disableFormControls(form: NgForm) {
    if (this.readonly) {
      // tslint:disable-next-line: forin
      for (const key in form.controls) {
        form.controls[key].disable();
      }
    }
  }

  /** 提交表单 */
  sfSubmit(...event: SFComponent[]) {
    for (const item of event) {
      item.validator({ emitError: true });
    }
    const valid = event.findIndex(o => !o.valid) === -1;
    if (this.readonly || !valid) {
      return;
    }

    this.submitForm(...event.filter(o => o.value).map(o => o.value));
  }

  /** 提交表单 */
  abstract submitForm(...event: any[]);
}

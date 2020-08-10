import {
  Component,
  forwardRef,
  Injector,
  Input,
  OnInit,
  SimpleChange,
  SimpleChanges,
  ViewChildren,
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ArrayService } from '@delon/util';
import { ClaimsServiceProxy } from '@service-proxies';
import { ControlComponentBase } from '@shared/common';
import * as _ from 'loadsh';
import { NzFormatEmitEvent, NzTreeComponent, NzTreeNodeOptions } from 'ng-zorro-antd';
import { NzTreeNode } from 'ng-zorro-antd/core/tree';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'permission-tree',
  templateUrl: './permission-tree.component.html',
  styleUrls: ['./permission-tree.component.less'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => PermissionTreeComponent),
    multi: true,
  }],
})
export class PermissionTreeComponent extends ControlComponentBase<string[]> implements OnInit {


  // @ViewChildren('tree') treeRef: NzTreeComponent;


  treeData: NzTreeNode[];

  treeSearchVal: string;

  constructor(
    injector: Injector,
    public arraySer: ArrayService,
    public claimsSer: ClaimsServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loading = true;
    this.claimsSer.getAllClaimsWithTree()
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {

        res.forEach(o => {
          o.claim = this.l(o.claim);
        });

        this.treeData = this.arraySer.arrToTreeNode(res, {
          idMapName: 'claim',
          parentIdMapName: 'parent',
          titleMapName: 'claim',
        });

        if (Array.isArray(this.value)) {
          this.writeValue(_.clone(this.value));
        }
      });
  }

  onAfterViewInit(): void {
  }

  onDestroy(): void {
  }

  onInit(): void {
  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {

  }

  onNzCheckBoxChange(event: NzFormatEmitEvent) {
    const checkedKeys = this.arraySer.getKeysByTreeNode(event.checkedKeys);
    this.emitValueChange(checkedKeys);
  }

}

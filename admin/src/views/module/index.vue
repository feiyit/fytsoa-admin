<script setup lang="ts">
import { computed, nextTick, ref } from "vue";
import { ElMessage } from "element-plus";

import HelloWorld from "@/component/HelloWorld.vue";
import FytSliderVerify from "@/component/FytSliderVerify.vue";
import SoaChart from "@/component/soaChart/index.vue";
import { createBarChartOptions } from "@/component/soaChart/useSoaChart";
// @ts-expect-error - soaCron currently lacks .d.ts module typing
import SoaCron from "@/component/soaCron/index.vue";
import { useSoaModal } from "@/component/soaModal/index.vue";
import { useSoaDrawer } from "@/component/soaDrawer/index.vue";
import SoaEditor from "@/component/soaEditor/index.vue";
import SoaForm from "@/component/soaForm/index.vue";
import type { FormConfig, FormRecord } from "@/component/soaForm/types";
import SoaFormTable from "@/component/soaFormTable/index.vue";
import SoaIcon from "@/component/soaIcon/index.vue";
import SoaSelect from "@/component/soaSelect/index.vue";
import SoaSelectAdmin from "@/component/soaSelectAdmin/index.vue";
import SoaTable from "@/component/soaTable/index.vue";
import SoaTableSelect from "@/component/soaTableSelect/index.vue";
import SoaUpload from "@/component/soaUpload/index.vue";
import SoaUploadMultiple from "@/component/soaUpload/multiple.vue";
import SoaUploadFile from "@/component/soaUpload/file.vue";
import SoaWorkflow from "@/component/soaWorkflow/index.vue";
import type { WorkflowNode } from "@/component/soaWorkflow/types";

defineOptions({ name: "ModuleComponentDemoView" });

const componentCatalog = [
  { id: "hello-world", name: "HelloWorld", scene: "基础组件与交互验证" },
  { id: "slider-verify", name: "FytSliderVerify", scene: "登录滑块安全验证" },
  { id: "soa-chart", name: "soaChart", scene: "图表可视化展示" },
  { id: "soa-cron", name: "soaCron", scene: "定时任务规则生成" },
  { id: "soa-modal", name: "soaModal", scene: "函数式弹窗调用" },
  { id: "soa-drawer", name: "soaDrawer", scene: "函数式抽屉调用" },
  { id: "soa-editor", name: "soaEditor", scene: "富文本内容编辑" },
  { id: "soa-form", name: "soaForm", scene: "Schema 配置化表单" },
  { id: "soa-form-table", name: "soaFormTable", scene: "动态明细表单" },
  { id: "soa-icon", name: "soaIcon", scene: "图标选择器" },
  { id: "soa-select", name: "soaSelect", scene: "扩展下拉选择" },
  { id: "soa-select-admin", name: "soaSelectAdmin", scene: "组织人员选择" },
  { id: "soa-table", name: "soaTable", scene: "统一表格组件" },
  { id: "soa-table-select", name: "soaTableSelect", scene: "表格下拉选择器" },
  { id: "soa-upload", name: "soaUpload", scene: "单图上传" },
  { id: "soa-upload-multiple", name: "soaUpload/multiple", scene: "多图上传" },
  { id: "soa-upload-file", name: "soaUpload/file", scene: "文件上传" },
  { id: "soa-workflow", name: "soaWorkflow", scene: "流程设计器" },
] as const;

const activeId = ref<string>(componentCatalog[0].id);
const scrollToSection = async (id: string) => {
  activeId.value = id;
  await nextTick();
  document.getElementById(id)?.scrollIntoView({ behavior: "smooth", block: "start" });
};
const syncActive = (id: string) => {
  activeId.value = id;
};

const copySectionCode = async (sectionId: string) => {
  const codeEl = document.querySelector<HTMLElement>(`#${sectionId} .demo-code code`);
  const code = codeEl?.textContent?.trim();
  if (!code) {
    ElMessage.warning("未找到可复制的代码");
    return;
  }
  try {
    if (navigator?.clipboard?.writeText) {
      await navigator.clipboard.writeText(code);
    } else {
      const input = document.createElement("textarea");
      input.value = code;
      document.body.appendChild(input);
      input.select();
      document.execCommand("copy");
      document.body.removeChild(input);
    }
    ElMessage.success("代码已复制");
  } catch (_error) {
    ElMessage.error("复制失败，请手动复制");
  }
};

const sliderPassed = ref(false);
const sliderRef = ref<{ resetSlider: () => void } | null>(null);

const chartOptions = computed(() =>
  createBarChartOptions({
    title: "近7日访问趋势",
    xAxis: ["周一", "周二", "周三", "周四", "周五", "周六", "周日"],
    series: [
      { name: "PV", data: [120, 220, 182, 234, 290, 330, 310] },
      { name: "UV", data: [90, 120, 132, 154, 200, 180, 210] },
    ],
  })
);

const cronExp = ref("0 0/10 * * * ?");

const [DemoModal, demoModalApi] = useSoaModal({
  onConfirm: () => {
    ElMessage.success("已确认弹窗操作");
    demoModalApi.close();
  },
});
const [DemoDrawer, demoDrawerApi] = useSoaDrawer({
  onConfirm: () => {
    ElMessage.success("已确认抽屉操作");
    demoDrawerApi.close();
  },
});

const editorContent = ref("<p><strong>soaEditor</strong> 演示内容。</p>");
const formModel = ref<FormRecord>({});
const formResult = ref("");
const formConfig: FormConfig = {
  labelWidth: 100,
  submitText: "提交演示",
  formItems: [
    { name: "title", label: "标题", component: "input", value: "组件演示", options: { placeholder: "请输入标题" } },
    {
      name: "category",
      label: "分类",
      component: "select",
      value: "frontend",
      options: { items: [{ label: "前端", value: "frontend" }, { label: "后端", value: "backend" }] },
    },
    { name: "publish", label: "是否发布", component: "switch", value: true },
    { name: "score", label: "评分", component: "rate", value: 4 },
  ],
};
const onFormSubmit = (value: FormRecord) => {
  formResult.value = JSON.stringify(value, null, 2);
};

const formTableRows = ref([
  { name: "需求评审", owner: "产品经理", days: 2 },
  { name: "接口联调", owner: "后端工程师", days: 3 },
]);
const formTableTemplate = { name: "", owner: "", days: 1 };

const iconValue = ref("lucide:settings");
const selectValue = ref<string | number>("vue");
const selectOptions = ref([
  { label: "Vue 3", value: "vue" },
  { label: "React", value: "react" },
  { label: "Angular", value: "angular" },
]);

const adminSingle = ref<Record<string, any> | null>(null);
const adminMultiple = ref<Record<string, any>[]>([]);
const getUserLabel = (user: Record<string, any> | null) => {
  if (!user) return "未选择";
  return user.fullName || user.displayName || user.userName || user.loginAccount || user.mobile || user.id;
};

const tableColumns = ref([
  { title: "ID", dataIndex: "id", key: "id", width: 80 },
  { title: "模块", dataIndex: "module", key: "module" },
  { title: "负责人", dataIndex: "owner", key: "owner" },
  { title: "状态", dataIndex: "status", key: "status" },
]);
const tableRows = ref([
  { id: 1, module: "权限中心", owner: "张三", status: "进行中" },
  { id: 2, module: "工作流", owner: "李四", status: "待测试" },
  { id: 3, module: "内容管理", owner: "王五", status: "已完成" },
]);

const tableSelectData = ref([
  { id: 1, name: "张三", dept: "研发部", role: "前端工程师" },
  { id: 2, name: "李四", dept: "研发部", role: "后端工程师" },
  { id: 3, name: "王五", dept: "产品部", role: "产品经理" },
]);
const tableSelectSingle = ref<Record<string, any> | null>(null);
const tableSelectMultiple = ref<Record<string, any>[]>([]);

const singleImagePath = ref("");
const multipleImagePaths = ref<
  Array<{ url: string; path: string; error?: boolean; uploading?: boolean }>
>([]);
const filePaths = ref<string[]>([]);

const workflowModel = ref<WorkflowNode | null>({
  nodeName: "发起人",
  type: 0,
  nodeRoleList: [],
  childNode: {
    nodeName: "审核人",
    type: 1,
    setType: 1,
    nodeUserList: [],
    nodeRoleList: [],
    examineLevel: 1,
    directorLevel: 1,
    selectMode: 1,
    termAuto: false,
    term: 0,
    termMode: 1,
    examineMode: 1,
    directorMode: 0,
    childNode: {
      nodeName: "抄送人",
      type: 2,
      userSelectFlag: true,
      nodeUserList: [],
      childNode: null,
    },
  },
});
</script>

<template>
  <div class="module-demo h-[calc(100vh-130px)]">
    <el-card
      shadow="never"
      class="module-sidebar rounded-[.5vw] !border-slate-200/80 dark:!border-slate-750"
    >
      <template #header>
        <div class="font-semibold">组件演示目录</div>
      </template>
      <el-scrollbar max-height="calc(100vh - 220px)">
        <div class="space-y-1 pr-2">
          <button
            v-for="item in componentCatalog"
            :key="item.id"
            type="button"
            class="w-full rounded-md px-3 py-2 text-left text-sm transition"
            :class="
              activeId === item.id
                ? 'bg-primary-50 text-primary-600 dark:bg-primary-500/10 dark:text-primary-300'
                : 'text-slate-600 hover:bg-slate-100 dark:text-slate-300 dark:hover:bg-slate-800'
            "
            @click="scrollToSection(item.id)"
          >
            <div class="font-medium">{{ item.name }}</div>
            <div class="text-xs opacity-70">{{ item.scene }}</div>
          </button>
        </div>
      </el-scrollbar>
    </el-card>

    <el-card
      shadow="never"
      class="module-content rounded-[.5vw] !border-slate-200/80 dark:!border-slate-750"
    >
      <template #header>
        <div class="font-semibold">Component 场景演示（说明 + 示例 + 用法）</div>
      </template>
      <el-scrollbar max-height="calc(100vh - 220px)">
        <div class="space-y-4 pr-2">
          <section id="hello-world" class="demo-section" @mouseenter="syncActive('hello-world')">
            <h3 class="demo-title">1. HelloWorld</h3>
            <p class="demo-desc">组件说明：用于验证基础渲染、响应式状态与事件绑定。</p>
            <p class="demo-scene">演示场景：新项目启动后快速检查前端运行状态。</p>
            <el-card shadow="never" class="demo-card">
              <HelloWorld msg="Hello FytSoa" />
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('hello-world')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;HelloWorld msg="Hello FytSoa" /&gt;</code></pre>
            </div>
          </section>

          <section id="slider-verify" class="demo-section" @mouseenter="syncActive('slider-verify')">
            <h3 class="demo-title">2. FytSliderVerify</h3>
            <p class="demo-desc">组件说明：滑块验证码，可在登录/关键操作前做人机校验。</p>
            <p class="demo-scene">演示场景：账号登录前置验证，提高接口安全性。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <FytSliderVerify ref="sliderRef" v-model="sliderPassed" />
              <div class="flex items-center gap-2">
                <el-tag :type="sliderPassed ? 'success' : 'warning'">{{ sliderPassed ? "已通过" : "未通过" }}</el-tag>
                <el-button size="small" @click="sliderRef?.resetSlider()">重置滑块</el-button>
              </div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('slider-verify')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;FytSliderVerify v-model="passed" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-chart" class="demo-section" @mouseenter="syncActive('soa-chart')">
            <h3 class="demo-title">3. soaChart</h3>
            <p class="demo-desc">组件说明：统一 ECharts 封装，支持柱状/折线/饼图等。</p>
            <p class="demo-scene">演示场景：仪表盘趋势分析、运营看板统计。</p>
            <el-card shadow="never" class="demo-card">
              <SoaChart :options="chartOptions" :height="320" />
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-chart')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaChart :options="chartOptions" :height="320" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-cron" class="demo-section" @mouseenter="syncActive('soa-cron')">
            <h3 class="demo-title">4. soaCron</h3>
            <p class="demo-desc">组件说明：可视化 CRON 规则编辑，降低手写表达式成本。</p>
            <p class="demo-scene">演示场景：任务调度、自动化巡检、消息推送周期配置。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaCron v-model="cronExp" />
              <el-alert type="info" :closable="false" title="当前表达式">
                <template #default>{{ cronExp }}</template>
              </el-alert>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-cron')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaCron v-model="cronExp" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-modal" class="demo-section" @mouseenter="syncActive('soa-modal')">
            <h3 class="demo-title">5. soaModal</h3>
            <p class="demo-desc">组件说明：函数式弹窗封装，统一确认/取消行为。</p>
            <p class="demo-scene">演示场景：删除确认、提交审批、二次校验。</p>
            <el-card shadow="never" class="demo-card">
              <el-button type="primary" @click="demoModalApi.open()">打开弹窗</el-button>
              <DemoModal title="soaModal 演示" width="520px">
                <div class="py-2">这里是弹窗内容区域。</div>
              </DemoModal>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-modal')">复制代码</el-button>
              <pre class="demo-code"><code>const [Modal, modalApi] = useSoaModal({ onConfirm: () =&gt; modalApi.close() })</code></pre>
            </div>
          </section>

          <section id="soa-drawer" class="demo-section" @mouseenter="syncActive('soa-drawer')">
            <h3 class="demo-title">6. soaDrawer</h3>
            <p class="demo-desc">组件说明：函数式抽屉封装，适合侧边详情编辑。</p>
            <p class="demo-scene">演示场景：资料详情、配置编辑、分步填写。</p>
            <el-card shadow="never" class="demo-card">
              <el-button type="primary" @click="demoDrawerApi.open()">打开抽屉</el-button>
              <DemoDrawer title="soaDrawer 演示" size="40%">
                <div class="py-2">这里是抽屉内容区域。</div>
              </DemoDrawer>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-drawer')">复制代码</el-button>
              <pre class="demo-code"><code>const [Drawer, drawerApi] = useSoaDrawer({ onConfirm: () =&gt; drawerApi.close() })</code></pre>
            </div>
          </section>

          <section id="soa-editor" class="demo-section" @mouseenter="syncActive('soa-editor')">
            <h3 class="demo-title">7. soaEditor</h3>
            <p class="demo-desc">组件说明：富文本编辑器，支持模板、图片上传与深色模式。</p>
            <p class="demo-scene">演示场景：文章发布、公告编辑、内容运营。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaEditor v-model="editorContent" :height="280" />
              <div class="text-xs text-slate-500">当前内容长度：{{ editorContent.length }}</div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-editor')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaEditor v-model="editorContent" :height="280" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-form" class="demo-section" @mouseenter="syncActive('soa-form')">
            <h3 class="demo-title">8. soaForm</h3>
            <p class="demo-desc">组件说明：Schema 驱动动态表单，支持快速配置业务字段。</p>
            <p class="demo-scene">演示场景：后台配置页、低代码表单构建。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaForm v-model="formModel" :config="formConfig" @submit="onFormSubmit" />
              <el-input v-if="formResult" type="textarea" :rows="4" readonly :model-value="formResult" />
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-form')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaForm v-model="formModel" :config="formConfig" @submit="onFormSubmit" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-form-table" class="demo-section" @mouseenter="syncActive('soa-form-table')">
            <h3 class="demo-title">9. soaFormTable</h3>
            <p class="demo-desc">组件说明：可增删行的表格式表单，支持明细数据录入。</p>
            <p class="demo-scene">演示场景：费用清单、任务拆分、采购明细。</p>
            <el-card shadow="never" class="demo-card">
              <SoaFormTable v-model="formTableRows" :add-template="formTableTemplate" :is-add="true">
                <el-table-column label="任务名称" prop="name" min-width="180">
                  <template #default="{ row }"><el-input v-model="row.name" /></template>
                </el-table-column>
                <el-table-column label="负责人" prop="owner" min-width="140">
                  <template #default="{ row }"><el-input v-model="row.owner" /></template>
                </el-table-column>
                <el-table-column label="工期(天)" prop="days" width="120">
                  <template #default="{ row }"><el-input-number v-model="row.days" :min="1" /></template>
                </el-table-column>
              </SoaFormTable>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-form-table')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaFormTable v-model="rows" :add-template="template"&gt;...&lt;/SoaFormTable&gt;</code></pre>
            </div>
          </section>

          <section id="soa-icon" class="demo-section" @mouseenter="syncActive('soa-icon')">
            <h3 class="demo-title">10. soaIcon</h3>
            <p class="demo-desc">组件说明：统一图标选择器，支持检索与手动输入。</p>
            <p class="demo-scene">演示场景：菜单图标配置、按钮图标配置。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaIcon v-model="iconValue" />
              <el-tag>当前图标：{{ iconValue }}</el-tag>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-icon')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaIcon v-model="iconValue" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-select" class="demo-section" @mouseenter="syncActive('soa-select')">
            <h3 class="demo-title">11. soaSelect</h3>
            <p class="demo-desc">组件说明：扩展 Select，支持远程、可新增选项等能力。</p>
            <p class="demo-scene">演示场景：字典项选择、标签选择。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaSelect v-model="selectValue" :data="selectOptions" filterable clearable />
              <div class="text-xs text-slate-500">当前值：{{ selectValue }}</div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-select')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaSelect v-model="selectValue" :data="options" filterable clearable /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-select-admin" class="demo-section" @mouseenter="syncActive('soa-select-admin')">
            <h3 class="demo-title">12. soaSelectAdmin</h3>
            <p class="demo-desc">组件说明：组织+人员弹窗选择器，支持单选与多选。</p>
            <p class="demo-scene">演示场景：审批人选择、抄送人选择、角色绑定。</p>
            <el-card shadow="never" class="demo-card space-y-4">
              <div>
                <div class="mb-2 text-xs font-medium text-slate-500">单选示例</div>
                <SoaSelectAdmin v-model="adminSingle" />
                <div class="mt-2 text-xs text-slate-500">当前单选：{{ getUserLabel(adminSingle) }}</div>
              </div>
              <div>
                <div class="mb-2 text-xs font-medium text-slate-500">多选示例</div>
                <SoaSelectAdmin v-model="adminMultiple" multiple />
                <div class="mt-2 text-xs text-slate-500">当前多选数量：{{ adminMultiple.length }}</div>
              </div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-select-admin')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaSelectAdmin v-model="selectedAdmins" multiple /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-table" class="demo-section" @mouseenter="syncActive('soa-table')">
            <h3 class="demo-title">13. soaTable</h3>
            <p class="demo-desc">组件说明：统一表格封装，内置分页、工具栏、选择能力。</p>
            <p class="demo-scene">演示场景：后台列表页、管理台数据查询。</p>
            <el-card shadow="never" class="demo-card">
              <div class="h-[420px]">
                <SoaTable
                  :columns="tableColumns"
                  :table-data="tableRows"
                  row-key="id"
                  :toolbar-show="false"
                  :pagination-show="false"
                />
              </div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-table')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaTable :columns="columns" :table-data="rows" row-key="id" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-table-select" class="demo-section" @mouseenter="syncActive('soa-table-select')">
            <h3 class="demo-title">14. soaTableSelect</h3>
            <p class="demo-desc">组件说明：下拉内嵌表格，适合复杂对象选择。</p>
            <p class="demo-scene">演示场景：人员选择、资产选择、项目选择。</p>
            <el-card shadow="never" class="demo-card space-y-3">
              <div>
                <div class="mb-2 text-xs font-medium text-slate-500">单选示例</div>
                <SoaTableSelect v-model="tableSelectSingle" :data="tableSelectData" value-key="id" label-key="name">
                  <el-table-column label="姓名" prop="name" min-width="120" />
                  <el-table-column label="部门" prop="dept" min-width="120" />
                  <el-table-column label="岗位" prop="role" min-width="140" />
                </SoaTableSelect>
              </div>
              <div>
                <div class="mb-2 text-xs font-medium text-slate-500">多选示例</div>
                <SoaTableSelect
                  v-model="tableSelectMultiple"
                  :data="tableSelectData"
                  value-key="id"
                  label-key="name"
                  multiple
                >
                  <el-table-column label="姓名" prop="name" min-width="120" />
                  <el-table-column label="部门" prop="dept" min-width="120" />
                  <el-table-column label="岗位" prop="role" min-width="140" />
                </SoaTableSelect>
              </div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-table-select')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaTableSelect v-model="picked" :data="list" multiple&gt;...&lt;/SoaTableSelect&gt;</code></pre>
            </div>
          </section>

          <section id="soa-upload" class="demo-section" @mouseenter="syncActive('soa-upload')">
            <h3 class="demo-title">15. soaUpload</h3>
            <p class="demo-desc">组件说明：单图上传组件，支持手动输入 URL。</p>
            <p class="demo-scene">演示场景：头像上传、封面图上传。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaUpload v-model="singleImagePath" :width="180" :height="120" directory="images" />
              <div class="text-xs text-slate-500 break-all">返回路径：{{ singleImagePath || "暂无" }}</div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-upload')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaUpload v-model="imagePath" directory="images" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-upload-multiple" class="demo-section" @mouseenter="syncActive('soa-upload-multiple')">
            <h3 class="demo-title">16. soaUpload/multiple</h3>
            <p class="demo-desc">组件说明：多图上传，支持预览、删除与数量限制。</p>
            <p class="demo-scene">演示场景：商品图库、案例集上传。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaUploadMultiple v-model="multipleImagePaths" :max-count="4" directory="images" />
              <div class="text-xs text-slate-500">已上传数量：{{ multipleImagePaths.length }}</div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-upload-multiple')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaUploadMultiple v-model="imageList" :max-count="4" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-upload-file" class="demo-section" @mouseenter="syncActive('soa-upload-file')">
            <h3 class="demo-title">17. soaUpload/file</h3>
            <p class="demo-desc">组件说明：文件上传组件，支持类型与大小校验。</p>
            <p class="demo-scene">演示场景：合同附件、文档资料上传。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <SoaUploadFile v-model="filePaths" directory="files" :limit="3" />
              <div class="text-xs text-slate-500">文件数量：{{ filePaths.length }}</div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-upload-file')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaUploadFile v-model="filePaths" directory="files" :limit="3" /&gt;</code></pre>
            </div>
          </section>

          <section id="soa-workflow" class="demo-section" @mouseenter="syncActive('soa-workflow')">
            <h3 class="demo-title">18. soaWorkflow</h3>
            <p class="demo-desc">组件说明：流程设计器，支持发起人/审批/抄送节点配置。</p>
            <p class="demo-scene">演示场景：审批流搭建、业务流程可视化。</p>
            <el-card shadow="never" class="demo-card space-y-2">
              <div class="overflow-auto rounded-md border border-slate-200 p-2 dark:border-slate-700">
                <SoaWorkflow v-model="workflowModel" />
              </div>
            </el-card>
            <div class="demo-code-wrap">
              <el-button class="copy-code-btn" size="small" text @click="copySectionCode('soa-workflow')">复制代码</el-button>
              <pre class="demo-code"><code>&lt;SoaWorkflow v-model="workflowModel" /&gt;</code></pre>
            </div>
          </section>
        </div>
      </el-scrollbar>
    </el-card>
  </div>
</template>

<style scoped>
.module-demo {
  display: grid;
  grid-template-columns: 240px minmax(0, 1fr);
  gap: 12px;
}

.module-sidebar {
  width: 240px;
  min-width: 240px;
}

.module-content {
  min-width: 0;
}

.demo-section {
  scroll-margin-top: 12px;
  border: 1px solid rgba(148, 163, 184, 0.25);
  border-radius: 12px;
  padding: 14px;
  background: rgba(248, 250, 252, 0.55);
}

.demo-title {
  margin: 0;
  font-size: 16px;
  font-weight: 600;
  color: #0f172a;
}

.demo-desc {
  margin-top: 8px;
  margin-bottom: 4px;
  font-size: 13px;
  color: #334155;
}

.demo-scene {
  margin-top: 0;
  margin-bottom: 10px;
  font-size: 13px;
  color: #475569;
}

.demo-card {
  border: 1px dashed rgba(148, 163, 184, 0.45);
  border-radius: 10px;
}

.demo-code-wrap {
  position: relative;
  margin-top: 10px;
}

.copy-code-btn {
  position: absolute;
  top: 4px;
  right: 8px;
  z-index: 1;
  color: #cbd5e1;
}

.demo-code {
  margin-top: 0;
  margin-bottom: 0;
  padding: 28px 10px 10px;
  border-radius: 8px;
  background: #0f172a;
  color: #e2e8f0;
  font-size: 12px;
  line-height: 1.5;
  overflow-x: auto;
}

:deep(html.dark) .demo-section {
  border-color: rgba(71, 85, 105, 0.7);
  background: rgba(15, 23, 42, 0.35);
}

:deep(html.dark) .demo-title {
  color: #e2e8f0;
}

:deep(html.dark) .demo-desc,
:deep(html.dark) .demo-scene {
  color: #cbd5e1;
}

:deep(html.dark) .copy-code-btn {
  color: #e2e8f0;
}
</style>

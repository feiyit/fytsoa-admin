<script setup lang="ts">
defineOptions({ name: "AboutFrontendView" });

type DependencyType = "dependencies" | "devDependencies";

interface DependencyRow {
  name: string;
  version: string;
  type: DependencyType;
}

interface StackBlock {
  title: string;
  detail: string;
}

// 与 admin/package.json 保持一致（用于页面内可视化展示）
const dependencies: Record<string, string> = {
  "@element-plus/icons-vue": "^2.3.2",
  echarts: "^6.0.0",
  "element-plus": "^2.11.8",
  vue: "^3.5.24",
  "vue-echarts": "^8.0.1",
};

const devDependencies: Record<string, string> = {
  "@iconify/vue": "5.0.0",
  "@shene/table": "1.0.0-alpha.24",
  "@tailwindcss/postcss": "^4.1.17",
  "@tinymce/tinymce-vue": "^6.3.0",
  "@types/crypto-js": "^4.2.2",
  "@types/node": "^24.10.1",
  "@types/nprogress": "^0.2.3",
  "@vitejs/plugin-vue": "^6.0.1",
  "@vue/tsconfig": "^0.8.1",
  "@vueuse/core": "^14.0.0",
  autoprefixer: "^10.4.22",
  axios: "^1.13.2",
  "crypto-js": "^4.2.0",
  dayjs: "1.11.19",
  nprogress: "^0.2.0",
  pinia: "^3.0.4",
  postcss: "^8.5.6",
  "sass-embedded": "^1.93.3",
  tailwindcss: "^3.4.18",
  tinymce: "^8.2.1",
  typescript: "~5.9.3",
  "unplugin-auto-import": "^20.2.0",
  "unplugin-vue-setup-extend-plus": "^1.0.1",
  vite: "^7.2.4",
  "vue-cropperjs": "^5.0.0",
  "vue-router": "^4.6.3",
  "vue-tsc": "^3.1.4",
  vuedraggable: "^4.1.0",
};

const dependencyRows = computed<DependencyRow[]>(() => {
  return Object.entries(dependencies)
    .map(([name, version]) => ({
      name,
      version,
      type: "dependencies" as const,
    }))
    .sort((a, b) => a.name.localeCompare(b.name));
});

const devDependencyRows = computed<DependencyRow[]>(() => {
  return Object.entries(devDependencies)
    .map(([name, version]) => ({
      name,
      version,
      type: "devDependencies" as const,
    }))
    .sort((a, b) => a.name.localeCompare(b.name));
});

const importantComponents = computed(() => [
  { name: "Vue 3", desc: "前端核心框架" },
  { name: "Vite", desc: "构建与开发服务器" },
  { name: "TypeScript", desc: "类型系统与工程可维护性" },
  { name: "Element Plus", desc: "企业级 UI 组件库" },
  { name: "Pinia", desc: "状态管理" },
  { name: "Vue Router", desc: "路由与动态菜单挂载" },
  { name: "Axios", desc: "HTTP 请求与统一拦截" },
  { name: "ECharts / Vue-ECharts", desc: "图表可视化" },
  { name: "TinyMCE", desc: "富文本编辑" },
  { name: "vuedraggable", desc: "拖拽交互" },
]);

const stackSummary = computed<StackBlock[]>(() => [
  {
    title: "架构模式",
    detail:
      "采用 Vue 3 + TypeScript + Vite 组合，结合 Pinia、Vue Router 与统一请求层，构建模块化、可维护、可扩展的前端工程。",
  },
  {
    title: "工程能力",
    detail:
      "通过 unplugin-auto-import、Vue Setup Extend、TailwindCSS 与 PostCSS 提升开发效率，同时保留类型检查与构建约束。",
  },
  {
    title: "业务能力",
    detail:
      "内置图表、富文本、上传、拖拽、权限指令等能力，支持中后台系统常见业务场景快速落地。",
  },
  {
    title: "接口安全",
    detail:
      "前端统一通过 Axios 拦截器附加 accessToken，并支持签名头、X-Refresh-Token 自动续期及 401 统一重登流程。",
  },
]);

const totalCount = computed(
  () => dependencyRows.value.length + devDependencyRows.value.length,
);
</script>

<template>
  <div class="space-y-3">
    <el-card
      shadow="never"
      class="rounded-[.5vw] !border-slate-200/80 dark:!border-slate-750"
    >
      <template #header>
        <div class="flex items-center justify-between">
          <span class="text-base font-semibold">关于前端框架</span>
          <el-tag type="info">来源：admin/package.json</el-tag>
        </div>
      </template>

      <el-descriptions :column="1" border>
        <el-descriptions-item label="前端项目">FytAdmin</el-descriptions-item>
        <el-descriptions-item label="技术体系"
          >Vue 3 + TypeScript + Vite + Element Plus</el-descriptions-item
        >
        <el-descriptions-item label="依赖总数"
          >{{ totalCount }}（dependencies +
          devDependencies）</el-descriptions-item
        >
      </el-descriptions>
    </el-card>

    <el-row :gutter="12" class="mt-2">
      <el-col :xs="24" :lg="12">
        <el-card
          shadow="never"
          class="rounded-[.5vw] !border-slate-200/80 dark:!border-slate-750 h-full"
        >
          <template #header>
            <span class="font-semibold">前端架构介绍</span>
          </template>
          <div class="space-y-3">
            <div
              v-for="item in stackSummary"
              :key="item.title"
              class="rounded-lg border border-slate-200/80 p-3 dark:border-slate-700"
            >
              <div class="text-sm font-semibold mb-1">{{ item.title }}</div>
              <div class="text-sm text-slate-500 dark:text-slate-300">
                {{ item.detail }}
              </div>
            </div>
          </div>
        </el-card>
      </el-col>

      <el-col :xs="24" :lg="12">
        <el-card
          shadow="never"
          class="rounded-[.5vw] !border-slate-200/80 dark:!border-slate-750 h-full"
        >
          <template #header>
            <span class="font-semibold">关键组件清单</span>
          </template>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-2">
            <div
              v-for="item in importantComponents"
              :key="item.name"
              class="rounded-lg border border-slate-200/80 p-3 dark:border-slate-700"
            >
              <div class="text-sm font-semibold">{{ item.name }}</div>
              <div class="text-xs text-slate-500 dark:text-slate-300">
                {{ item.desc }}
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-card
      shadow="never"
      class="rounded-[.5vw] !border-slate-200/80 dark:!border-slate-750"
    >
      <template #header>
        <span class="font-semibold">Dependencies（运行时依赖）</span>
      </template>
      <el-table :data="dependencyRows" stripe>
        <el-table-column prop="name" label="包名" min-width="220" />
        <el-table-column prop="version" label="版本" min-width="130" />
      </el-table>
    </el-card>

    <el-card
      shadow="never"
      class="rounded-[.5vw] !border-slate-200/80 dark:!border-slate-750"
    >
      <template #header>
        <span class="font-semibold">DevDependencies（开发依赖）</span>
      </template>
      <el-table :data="devDependencyRows" stripe>
        <el-table-column prop="name" label="包名" min-width="220" />
        <el-table-column prop="version" label="版本" min-width="130" />
      </el-table>
    </el-card>
  </div>
</template>

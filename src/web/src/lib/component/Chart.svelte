<script lang="ts">
	import * as echarts from 'echarts';
	import { onDestroy, onMount } from 'svelte';

	// Create the echarts instance
	let _class: string;
	export { _class as class };

	export let title: string;
	export let options: echarts.EChartsOption;

	let chart: HTMLElement | null;
	let resizeObserver: ResizeObserver;
	let echart: echarts.ECharts | null;

	onMount(() => {
		console.log(options);
		echart = echarts.init(chart);
		echart.setOption({
			title: {
				text: title
			},
			...options
		});
		resizeObserver = new ResizeObserver(() => {
			echart?.resize();
		});
		resizeObserver.observe(chart!);
	});

	onDestroy(() => {
		resizeObserver?.disconnect();
		echart?.clear();
	});
</script>

<div bind:this={chart} class="w-full {_class}"></div>

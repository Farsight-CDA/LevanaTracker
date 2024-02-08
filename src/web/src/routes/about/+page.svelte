<script lang="ts">
	import ColorCard from '$lib/component/ColoredCard.svelte';

	let formation = true;
</script>

<div class="flex flex-wrap">
	<div class="w-full bg-gray-800 py-6 px-6">
		<div class="flex flex-wrap justify-between items-center pb-8">
			<!-- svelte-ignore a11y-click-events-have-key-events -->
			<!-- svelte-ignore a11y-no-static-element-interactions -->
			<div class="flex items-center mt-4 gap-1">
				<button
					on:click={() => (formation = false)}
					class="text-white {formation == true ? 'bg-transparent' : 'bg-gray-700'} p-2 rounded-lg"
				>
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
					>
						<line x1="8" y1="6" x2="21" y2="6" />
						<line x1="8" y1="12" x2="21" y2="12" />
						<line x1="8" y1="18" x2="21" y2="18" />
						<line x1="3" y1="6" x2="3.01" y2="6" />
						<line x1="3" y1="12" x2="3.01" y2="12" />
						<line x1="3" y1="18" x2="3.01" y2="18" />
					</svg>
				</button>
				<button
					on:click={() => (formation = true)}
					class="text-white {formation == false ? 'bg-transparent' : 'bg-gray-700'}  p-2 rounded-lg"
				>
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="20"
						height="20"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
					>
						<rect x="3" y="3" width="7" height="7" />
						<rect x="14" y="3" width="7" height="7" />
						<rect x="14" y="14" width="7" height="7" />
						<rect x="3" y="14" width="7" height="7" />
					</svg>
				</button>
			</div>
		</div>
		<div class="flex {formation === true ? 'flex-wrap' : 'flex-col'} ">
			<ColorCard seed={Math.random()}>aaaaaaaaaaaaa</ColorCard>
			<ColorCard seed={Math.random()}>aaaaaaaaaaaaa</ColorCard>
			<ColorCard seed={Math.random()}>aaaaaaaaaaaaa</ColorCard>
			<ColorCard seed={Math.random()}>aaaaaaaaaaaaa</ColorCard>
			<ColorCard seed={Math.random()}>aaaaaaaaaaaaa</ColorCard>
		</div>
	</div>
</div>
